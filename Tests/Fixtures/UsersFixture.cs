using Application.Models;
using Bogus;
using Domain.Entities;
using Domain.Enums;

namespace Tests.Fixtures
{
    public static class UsersFixture
    {

        public static UserDto CreateFakeUserDto()
        {
            var userDto = new Faker<UserDto>()
                .CustomInstantiator(f => new UserDto(
                    f.Person.FirstName,
                    f.Person.LastName,
                    f.Person.Email,
                    f.Random.AlphaNumeric(8),
                    EProfile.Admin
                )).Generate();

            return userDto;
        }
        
        public static User CreateFakeUser()
        {
            var user = new Faker<User>()
                .CustomInstantiator( f => new User(EProfile.Admin))
                .RuleFor(u => u.Name, f => f.Person.FirstName)
                .RuleFor(u => u.LastName, f => f.Person.LastName)
                .RuleFor(u => u.Email, f => f.Person.Email)
                .RuleFor(u => u.PasswordHash, f => f.Random.Hash())
                .Generate();

            return user;
        }
    }
}
