using System.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lib.services;
using Lib.models;

namespace PerformanceTest
{
    [TestClass]
    public class AuthPerformanceTest
    {
        [TestMethod]
        public void Login_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                AuthService.Login("radit@mail.com", "qwerty");
                Session.Instance.Account = null;
            }
            sw.Stop();

            Console.WriteLine($"Login 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Login 1000x terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void Login_InvalidCredentials_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                AuthService.Login("wrong@mail.com", "wrong");
            }
            sw.Stop();

            Console.WriteLine($"Login gagal 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Login gagal 1000x terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }
    }

    [TestClass]
    public class UserSearchPerformanceTest
    {
        [TestMethod]
        public void SearchUser_ByEmail_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                UserService.SearchUser("email", "radit@mail.com");
            }
            sw.Stop();

            Console.WriteLine($"Search user by email 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Search by email terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void SearchUser_ByName_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                UserService.SearchUser("name", "Raditya Atallahasyrif Rachmadie");
            }
            sw.Stop();

            Console.WriteLine($"Search user by name 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Search by name terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void SearchUser_NotFound_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                UserService.SearchUser("email", "notfound@mail.com");
            }
            sw.Stop();

            Console.WriteLine($"Search user not found 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Search not found terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }
    }

    [TestClass]
    public class PaperPerformanceTest
    {
        [TestInitialize]
        public void Setup()
        {
            Session.Instance.Account = null;
        }

        [TestMethod]
        public void SearchPaper_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                PaperService.SearchPaper("Lorem Ipsum: An Analysis");
            }
            sw.Stop();

            Console.WriteLine($"Search paper 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Search paper terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void UploadPaper_Performance()
        {
            AuthService.Login("radit@mail.com", "qwerty");

            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                PaperService.UploadPaper($"Performance Test Paper {i}", "Abstract content for performance testing.");
            }
            sw.Stop();

            Console.WriteLine($"Upload paper 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Upload paper terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void CanEdit_Performance()
        {
            AuthService.Login("fadiil@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");
            Assert.IsNotNull(paper);

            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                PaperService.CanEdit(paper);
            }
            sw.Stop();

            Console.WriteLine($"CanEdit check 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"CanEdit terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void GetAllPapers_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                var papers = DataContext.Papers.GetAll().ToList();
            }
            sw.Stop();

            Console.WriteLine($"GetAll papers 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"GetAll papers terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }
    }

    [TestClass]
    public class SessionPerformanceTest
    {
        [TestMethod]
        public void SessionInstance_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 10000; i++)
            {
                var s = Session.Instance;
            }
            sw.Stop();

            Console.WriteLine($"Session.Instance access 10000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 1000, $"Session.Instance terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }

        [TestMethod]
        public void LoginLogout_Cycle_Performance()
        {
            Stopwatch sw = new Stopwatch();

            sw.Start();
            for (int i = 0; i < 1000; i++)
            {
                AuthService.Login("radit@mail.com", "qwerty");
                Session.Instance.Logout();
            }
            sw.Stop();

            Console.WriteLine($"Login-Logout cycle 1000x: {sw.ElapsedMilliseconds} ms");
            Assert.IsTrue(sw.ElapsedMilliseconds < 5000, $"Login-Logout cycle terlalu lambat: {sw.ElapsedMilliseconds} ms");
        }
    }
}
