namespace Get_Together_Riders.Models
{
    public interface IRiderRepository
    {
        IEnumerable<Rider> GetAllRiders();
        Rider GetRiderById(int riderId);
        Rider GetRiderByEmail(string email);
        void AddRider(Rider rider);
        void UpdateRider(Rider rider);
        void DeleteRider(int riderId);
        void SaveChanges();
    }
}