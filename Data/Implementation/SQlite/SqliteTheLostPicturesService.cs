using Data.Interfaces;
using Data.Models;
using Microsoft.Extensions.Configuration;

namespace Data.Implementation.SQlite
{
    public class SqliteTheLostPicturesService : SqliteBaseService<TheLostPictures>, ITheLostPicturesService
    {
        public SqliteTheLostPicturesService(IConfigurationRoot configurationRoot) : base(configurationRoot)
        {
        }
    }
}