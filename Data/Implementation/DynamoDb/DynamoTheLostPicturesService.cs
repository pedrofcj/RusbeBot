using Data.Interfaces;
using Data.Models;

namespace Data.Implementation.DynamoDb
{
    public class DynamoTheLostPicturesService : DynamoBaseService<TheLostPictures>, ITheLostPicturesService
    {
    }
}