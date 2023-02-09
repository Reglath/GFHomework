using Homework.Contexts;

namespace Homework.Services
{
    public class Service
    {
        private ApplicationDbContext context;
        private IConfiguration config;


        public Service(ApplicationDbContext context, IConfiguration config)
        {
            this.context = context;
            this.config = config;
        }
    }
}
