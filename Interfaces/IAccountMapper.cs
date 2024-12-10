using Egzaminas.Dtos.Requests;
using Egzaminas.Entities;

namespace Egzaminas.Interfaces
{
    public interface IAccountMapper
    {
        Account Map(AccountRequestDto dto);
    }
}