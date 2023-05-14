using Microsoft.EntityFrameworkCore;

namespace Get_Together_Riders.Models
{
    public class RiderRepository : IRiderRepository
    {
        private readonly GTRDbContext _gTRDbContext;

        public RiderRepository(GTRDbContext gTRDbContext)
        {
            _gTRDbContext = gTRDbContext;
        }

        public void AddRider(Rider rider)
        {
            _gTRDbContext.Riders.Add(rider);
            _gTRDbContext.SaveChanges();
        }

        public Rider GetRiderById(int id)
        {
            return _gTRDbContext.Riders.FirstOrDefault(r => r.RiderID == id);
        }

        public Rider GetRiderByEmail(string email)
        {
            return _gTRDbContext.Riders.FirstOrDefault(r => r.Email == email);
        }

        public IEnumerable<Rider> GetAllRiders()
        {
            return _gTRDbContext.Set<Rider>().ToList();
        }

        public void UpdateRider(Rider rider)
        {
            _gTRDbContext.Riders.Update(rider);
            _gTRDbContext.SaveChanges();
        }

        public void DeleteRider(int id)
        {
            var rider = GetRiderById(id);
            if (rider != null)
            {
                _gTRDbContext.Riders.Remove(rider);
                _gTRDbContext.SaveChanges();
            }
        }

        public void SaveChanges()
        {
            _gTRDbContext.SaveChanges();
        }

        public Rider GetRiderByIdentityId(string identityId)
        {
            return _gTRDbContext.Riders.FirstOrDefault(r => r.IdentityUserId == identityId);
        }
    }

}
