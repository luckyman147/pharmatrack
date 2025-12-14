namespace PharmaTrack.Domain.enums;

public enum TransactionReason
{
    ManualAdjustment,      // Modified manually by admin
    CommandValidated,      // Automatic decrease due to command success
    CommandCancelled,      // Automatic increase due to command cancellation
    InventoryCheck,        // "NewChecking" / Inventory adjustment
    StockReturn            // Returned items
}
