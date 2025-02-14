namespace BulkyBook.DependencyInjection.Services
{
    public class ScopedGuidService : IScopedGuidService
    {
        private readonly Guid _id;
        public ScopedGuidService()
        {
            _id = Guid.NewGuid();
        }
        public string GetGuid()
        {
            return _id.ToString();
        }
    }
}
