using System;
using System.Linq;
using System.Threading.Tasks;
using backend.Controllers;
using backend.Models;
using backend.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace TestProject
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {

        }

        /// <summary>
        /// Tworzy nowy kontekst bazy danych InMemory o unikalnej nazwie.
        /// Dzięki temu każdy test ma czystą, niezależną bazę.
        /// </summary>
        private static HotelDbContext CreateInMemoryContext()
        {
            var options = new DbContextOptionsBuilder<HotelDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new HotelDbContext(options);
        }

        /// <summary>
        /// Test sprawdza, że logowanie pracownika kończy się powodzeniem przy poprawnych danych.
        /// - Tworzy pracownika z zahaszowanym hasłem "admin"
        /// - Wywołuje endpoint logowania i oczekuje 200 OK
        /// </summary>
        [Fact]
        public async Task Employee_Login_Succeeds_With_CorrectCredentials()
        {
            using var db = CreateInMemoryContext();

            // Przygotuj hasło i zasiej pracownika do bazy InMemory
            PasswordHasher.CreatePasswordHash("admin", out var hash, out var salt);
            var emp = new Employee { Name = "Test", Login = "test", PasswordHash = hash, PasswordSalt = salt };
            db.Employees.Add(emp);
            await db.SaveChangesAsync();

            var controller = new EmployeeController(db);
            var req = new EmployeeController.LoginRequest { Login = "test", Password = "admin" };

            // Wywołanie akcji logowania
            var result = await controller.Login(req);

            // Oczekujemy rezultatu 200 OK i zwróconego obiektu pracownika
            Assert.IsType<OkObjectResult>(result.Result);
            var ok = (OkObjectResult)result.Result;
            Assert.NotNull(ok.Value);
        }

        /// <summary>
        /// Test sprawdza, że logowanie nie powiedzie się przy błędnym haśle.
        /// - Przygotowuje pracownika z hasłem "admin"
        /// - Przekazuje złe hasło i oczekuje 401 Unauthorized
        /// </summary>
        [Fact]
        public async Task Employee_Login_Fails_With_WrongPassword()
        {
            using var db = CreateInMemoryContext();

            // Seed pracownika z poprawnym hasłem
            PasswordHasher.CreatePasswordHash("admin", out var hash, out var salt);
            db.Employees.Add(new Employee { Name = "Test", Login = "test", PasswordHash = hash, PasswordSalt = salt });
            await db.SaveChangesAsync();

            var controller = new EmployeeController(db);
            var req = new EmployeeController.LoginRequest { Login = "test", Password = "wrong" };

            // Wywołanie akcji logowania z nieprawidłowym hasłem
            var result = await controller.Login(req);

            // Oczekujemy 401 Unauthorized
            Assert.IsType<UnauthorizedObjectResult>(result.Result);
        }

        /// <summary>
        /// Test waliduje, że tworzenie rezerwacji zwraca konflikt (409),
        /// gdy istnieje już kolidująca rezerwacja dla tego samego pokoju.
        /// </summary>
        [Fact]
        public async Task Reservation_Create_Returns_Conflict_For_OverlappingReservation()
        {
            using var db = CreateInMemoryContext();

            // Zasiej niezbędne dane: gość, typ pokoju, pokój
            var guest = new Guest { FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123456789" };
            db.Guests.Add(guest);

            var roomType = new RoomType { Name = "T", PricePerNight = 10 };
            db.RoomTypes.Add(roomType);
            await db.SaveChangesAsync();

            var room = new Room { Number = "1A", RoomTypeId = roomType.Id };
            db.Rooms.Add(room);
            await db.SaveChangesAsync();

            // Istniejąca rezerwacja obejmuje zakres 2025-12-05 .. 2025-12-10
            var existing = new Reservation
            {
                GuestId = guest.Id,
                RoomId = room.Id,
                CheckInDate = new DateTime(2025, 12, 05),
                CheckOutDate = new DateTime(2025, 12, 10),
                TotalPrice = 100
            };
            db.Reservations.Add(existing);
            await db.SaveChangesAsync();

            var controller = new ReservationController(db);

            // Próbujemy utworzyć rezerwację nakładającą się częściowo => oczekujemy Conflict
            var dto = new ReservationController.ReservationCreateDto
            {
                GuestId = guest.Id,
                RoomId = room.Id,
                CheckInDate = new DateTime(2025, 12, 08),
                CheckOutDate = new DateTime(2025, 12, 12),
                TotalPrice = 120
            };

            var result = await controller.Create(dto);

            Assert.IsType<ConflictObjectResult>(result.Result);
        }

        /// <summary>
        /// Test sprawdza, że rezerwacja z nieprawidłowymi datami (checkOut == checkIn) zwraca BadRequest.
        /// </summary>
        [Fact]
        public async Task Reservation_Create_Returns_BadRequest_For_InvalidDates()
        {
            using var db = CreateInMemoryContext();

            // Zasiewamy minimalne dane niezbędne do próby utworzenia rezerwacji
            var guest = new Guest { FirstName = "A", LastName = "B", Email = "a@b.com", PhoneNumber = "123456789" };
            db.Guests.Add(guest);

            var roomType = new RoomType { Name = "T", PricePerNight = 10 };
            db.RoomTypes.Add(roomType);
            await db.SaveChangesAsync();

            var room = new Room { Number = "1A", RoomTypeId = roomType.Id };
            db.Rooms.Add(room);
            await db.SaveChangesAsync();

            var controller = new ReservationController(db);

            // Sprawdzenie dat równych -> niepoprawne
            var dto = new ReservationController.ReservationCreateDto
            {
                GuestId = guest.Id,
                RoomId = room.Id,
                CheckInDate = new DateTime(2025, 12, 10),
                CheckOutDate = new DateTime(2025, 12, 10), // invalid: equal
                TotalPrice = 0
            };

            var result = await controller.Create(dto);

            Assert.IsType<BadRequestObjectResult>(result.Result);

        }

        /// <summary>
        /// Test sprawdza, że metoda zwracająca dostępne pokoje poprawnie filtruje zajęte pokoje.
        /// - Tworzy dwa pokoje, a następnie rezerwację dla pierwszego w badanym przedziale.
        /// - Oczekuje, że w zwróconej kolekcji znajdzie się tylko drugi pokój.
        /// </summary>
        [Fact]
        public async Task Room_GetAvailable_Returns_Only_Available_Rooms()
        {
            using var db = CreateInMemoryContext();

            // Utwórz typ pokoju i dwa pokoje
            var roomType = new RoomType { Name = "T", PricePerNight = 10 };
            db.RoomTypes.Add(roomType);
            await db.SaveChangesAsync();

            var roomA = new Room { Number = "A1", RoomTypeId = roomType.Id };
            var roomB = new Room { Number = "B1", RoomTypeId = roomType.Id };
            db.Rooms.AddRange(roomA, roomB);
            await db.SaveChangesAsync();

            var guest = new Guest { FirstName = "G", LastName = "H", Email = "g@h.com", PhoneNumber = "987654321" };
            db.Guests.Add(guest);
            await db.SaveChangesAsync();

            // Rezerwacja dla roomA pokrywa badany okres -> roomA powinien być niedostępny
            db.Reservations.Add(new Reservation
            {
                GuestId = guest.Id,
                RoomId = roomA.Id,
                CheckInDate = new DateTime(2025, 12, 05),
                CheckOutDate = new DateTime(2025, 12, 08),
                TotalPrice = 100
            });
            await db.SaveChangesAsync();

            var controller = new RoomController(db);

            // Zapytanie o dostępność w okresie 2025-12-06 .. 2025-12-07
            var res = await controller.GetAvailable(new DateTime(2025, 12, 06), new DateTime(2025, 12, 07));
            Assert.IsType<OkObjectResult>(res.Result);
            var ok = (OkObjectResult)res.Result;

            // Sprawdź, że zwrócony został tylko drugi pokój
            var rooms = Assert.IsAssignableFrom<System.Collections.Generic.IEnumerable<Room>>(ok.Value);
            var roomList = rooms.ToList();

            Assert.Single(roomList);
            Assert.Equal(roomB.Id, roomList[0].Id);
        }
    }
}
