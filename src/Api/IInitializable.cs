using System.Threading.Tasks;

namespace Api
{
    internal interface IInitializable
    {
        Task Initialize();
    }
}