using System.ComponentModel.DataAnnotations;

namespace Ewallet.Application.DTOs;

public sealed record TopUpRequest(
    [Range(1, int.MaxValue)]
    int PaymentMethodId,

    [Range(typeof(decimal), "1", "999999999999")]
    decimal Amount
);

public sealed record PaymentMethodResponse(
    int Id, string Code, string Name
);

public sealed record TopUpResponse
(
    long Id, 
    long TransactionId, 
    long WalletId, 
    PaymentMethodResponse PaymentMethod, 
    decimal Amount, 
    string Status, 
    DateTime CreatedAtUtc, 
    DateTime? ExpiresAtUtc
);

public sealed record TransactionsTopUp(
    long Id, 
    string ReferenceNo,  
    long ReceiverWalletId, 
    string Type, 
    decimal Amount, 
    string Status, 
    TopUpSummaryResponse? TopUp
);

public sealed record TopUpSummaryResponse(
    long Id,
    PaymentMethodResponse PaymentMethod,
    DateTimeOffset? ExpiresAtUtc
);


public sealed record PaymentMethodDetailsResponse(
    int Id,
    string Code,
    string Name,
    bool IsActive
);



public sealed record TransactionsTransferResponse(
    long Id, 
    string ReferenceNo, 
    long? SenderWalletId, 
    long ReceiverWalletId, 
    string Type, 
    decimal Amount, 
    string Status, 
    DateTime CreatedAtUtc,
    DateTime? CompletedAtUtc
);
