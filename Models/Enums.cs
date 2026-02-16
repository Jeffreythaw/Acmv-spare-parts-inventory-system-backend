
namespace AcmvInventory.Models
{
    public enum UserRole { Admin, Storekeeper, Technician, Viewer }
    public enum PartStatus { Spare, Installed, Faulty, Obsolete }
    public enum Criticality { High, Medium, Low }
    public enum TxnType { ISSUE, RETURN, RECEIVE, ADJUSTMENT }
    public enum PRStatus { DRAFT, SUBMITTED, APPROVED, REJECTED, CANCELLED }
    public enum POStatus { DRAFT, SENT, PARTIALLY_RECEIVED, CLOSED, CANCELLED }
}
