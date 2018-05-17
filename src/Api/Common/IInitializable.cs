using System.Threading.Tasks;

namespace Api.Common
{
    internal interface IInitializable
    {
        Task Initialize();
    }
}