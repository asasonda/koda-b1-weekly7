using Ewallet.Application.DTOs;
using Ewallet.Application.Repositories;

namespace Ewallet.Application.Services;

public sealed class WalletHistoryService(
    IWalletHistoryRepository repository) : IWalletHistoryService
{
    private readonly IWalletHistoryRepository _repository = repository;

    public async Task<PagedResult<TransactionHistoryResponse>> GetTransactionsAsync(long walletId, TransactionHistoryRequest request,CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidatePagination(walletId, request.Page, request.PageSize);
        ValidateDateRange(request.FromDate, request.ToDate);

        bool walletExists = await _repository.WalletExistsAsync(walletId, ct);

        if (!walletExists)
        {
            throw new KeyNotFoundException("Wallet tidak ditemukan");
        }

        string? referenceNo = Normalize(request.ReferenceNo);
        string? type = Normalize(request.Type)?.ToUpperInvariant();
        string? status = Normalize(request.Status)?.ToUpperInvariant();

        string sort = NormalizeSort(request.Sort);

        var criteria = new TransactionHistoryCriteria(
            referenceNo,
            type,
            status,
            request.FromDate,
            request.ToDate,
            sort,
            request.Page,
            request.PageSize
        );

        return await _repository.GetTransactionsAsync(walletId, criteria, ct);
    }

    public async Task<PagedResult<WalletMutationHistoryResponse>> GetMutationsAsync(long walletId,WalletMutationHistoryRequest request,CancellationToken ct)
    {
        ArgumentNullException.ThrowIfNull(request);

        ValidatePagination(walletId, request.Page, request.PageSize);
        ValidateDateRange(request.FromDate, request.ToDate);

        bool walletExists = await _repository.WalletExistsAsync(walletId, ct);

        if (!walletExists)
        {
            throw new KeyNotFoundException("Wallet tidak ditemukan");
        }

        string? referenceNo = Normalize(request.ReferenceNo);
        string? direction = Normalize(request.Direction)?.ToUpperInvariant();

        if (direction is not null && direction is not ("IN" or "OUT"))
        {
            throw new ArgumentException("Direction harus IN atau OUT");
        }

        string sort = NormalizeSort(request.Sort);

        var criteria = new WalletMutationHistoryCriteria(
            referenceNo,
            direction,
            request.FromDate,
            request.ToDate,
            sort,
            request.Page,
            request.PageSize
        );

        return await _repository.GetMutationsAsync(walletId, criteria, ct);
    }

    private static void ValidatePagination(long walletId, int page, int pageSize)
    {
        if (walletId <= 0)
        {
            throw new ArgumentException("Wallet ID tidak valid");
        }

        if (page is < 1 or > 1000 || pageSize is < 1 or > 50)
        {
            throw new ArgumentOutOfRangeException(
                nameof(page),
                "Pagination tidak valid");
        }
    }

    private static void ValidateDateRange(DateTime? fromDate, DateTime? toDate)
    {
        if (fromDate.HasValue && toDate.HasValue && fromDate.Value > toDate.Value)
        {
            throw new ArgumentException("Rentang tanggal tidak valid");
        }
    }

    private static string? Normalize(string? value)
    {
        return string.IsNullOrWhiteSpace(value)
            ? null
            : value.Trim();
    }

    private static string NormalizeSort(string? sort)
    {
        return sort?.Trim().ToLowerInvariant() switch
        {
            "date_asc" => "date_asc",
            "date_desc" => "date_desc",
            "amount_asc" => "amount_asc",
            "amount_desc" => "amount_desc",

            _ => throw new ArgumentException("Sort tidak dikenali")
        };
    }
}