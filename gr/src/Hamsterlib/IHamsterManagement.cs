namespace HSRM.CS.DistributedSystems.Hamster
{
    public interface IHamsterManagement
    {
        ushort Collect(string ownerName);
        ushort GiveTreats(int id, ushort treats);
        HamsterState Howsdoing(int id);
        int Lookup(string ownerName, string hamsterName);
        int NewHamster(string ownerName, string hamsterName, ushort treats);
        ushort ReadEntry(int id, out string ownerName, out string hamsterName, out ushort price);
        IEnumerable<int> Search(string? ownerName = null, string? hamsterName = null);
    }
}