using Microsoft.VisualStudio.TestTools.UnitTesting;
using Lib.services;
using Lib.models;

namespace UnitTest
{
    [TestClass]
    public class AuthServiceTest
    {
        [TestInitialize]
        public void Setup()
        {
            Session.Instance.Account = null;
        }

        [TestMethod]
        public void Login_ValidCredentials_ReturnsTrue()
        {
            bool result = AuthService.Login("radit@mail.com", "qwerty");

            Assert.IsTrue(result);
            Assert.IsNotNull(Session.Instance.Account);
            Assert.AreEqual("radit@mail.com", Session.Instance.Account.Email);
        }

        [TestMethod]
        public void Login_InvalidEmail_ReturnsFalse()
        {
            bool result = AuthService.Login("notexist@mail.com", "qwerty");

            Assert.IsFalse(result);
            Assert.IsNull(Session.Instance.Account);
        }

        [TestMethod]
        public void Login_InvalidPassword_ReturnsFalse()
        {
            bool result = AuthService.Login("radit@mail.com", "wrongpassword");

            Assert.IsFalse(result);
            Assert.IsNull(Session.Instance.Account);
        }

        [TestMethod]
        public void Login_EmptyEmail_ReturnsFalse()
        {
            bool result = AuthService.Login("", "qwerty");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void Login_EmptyPassword_ReturnsFalse()
        {
            bool result = AuthService.Login("radit@mail.com", "");

            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class UserServiceTest
    {
        [TestMethod]
        public void SearchUser_ByEmail_Found()
        {
            User? user = UserService.SearchUser("email", "radit@mail.com");

            Assert.IsNotNull(user);
            Assert.AreEqual("radit@mail.com", user.Email);
        }

        [TestMethod]
        public void SearchUser_ByName_Found()
        {
            User? user = UserService.SearchUser("name", "Raditya Atallahasyrif Rachmadie");

            Assert.IsNotNull(user);
            Assert.AreEqual("Raditya Atallahasyrif Rachmadie", user.Name);
        }

        [TestMethod]
        public void SearchUser_ByEmail_NotFound()
        {
            User? user = UserService.SearchUser("email", "nobody@mail.com");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void SearchUser_ByName_NotFound()
        {
            User? user = UserService.SearchUser("name", "Nobody");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void SearchUser_InvalidField_ReturnsNull()
        {
            User? user = UserService.SearchUser("phone", "12345");

            Assert.IsNull(user);
        }

        [TestMethod]
        public void SearchUser_CaseInsensitive()
        {
            User? user = UserService.SearchUser("email", "RADIT@MAIL.COM");

            Assert.IsNotNull(user);
            Assert.AreEqual("radit@mail.com", user.Email);
        }
    }

    [TestClass]
    public class PaperServiceTest
    {
        [TestInitialize]
        public void Setup()
        {
            Session.Instance.Account = null;
        }

        [TestMethod]
        public void SearchPaper_Found()
        {
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");

            Assert.IsNotNull(paper);
            Assert.AreEqual("Lorem Ipsum: An Analysis", paper.Title);
        }

        [TestMethod]
        public void SearchPaper_NotFound()
        {
            Paper? paper = PaperService.SearchPaper("Nonexistent Paper Title");

            Assert.IsNull(paper);
        }

        [TestMethod]
        public void SearchPaper_CaseInsensitive()
        {
            Paper? paper = PaperService.SearchPaper("lorem ipsum: an analysis");

            Assert.IsNotNull(paper);
        }

        [TestMethod]
        public void UploadPaper_WithoutLogin_ReturnsFalse()
        {
            bool result = PaperService.UploadPaper("Test Paper", "Test Abstract");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void UploadPaper_WithLogin_ReturnsTrue()
        {
            AuthService.Login("radit@mail.com", "qwerty");

            bool result = PaperService.UploadPaper("New Paper Title", "New Abstract Content");

            Assert.IsTrue(result);

            Paper? uploaded = PaperService.SearchPaper("New Paper Title");
            Assert.IsNotNull(uploaded);
            Assert.AreEqual("Raditya Atallahasyrif Rachmadie", uploaded.Author);
        }

        [TestMethod]
        public void CanEdit_OwnerCanEdit()
        {
            AuthService.Login("fadiil@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");

            Assert.IsNotNull(paper);
            Assert.IsTrue(PaperService.CanEdit(paper));
        }

        [TestMethod]
        public void CanEdit_NonOwnerCannotEdit()
        {
            AuthService.Login("bacul@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");

            Assert.IsNotNull(paper);
            Assert.IsFalse(PaperService.CanEdit(paper));
        }

        [TestMethod]
        public void CanEdit_AdminCanEdit()
        {
            AuthService.Login("admin@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");

            Assert.IsNotNull(paper);
            Assert.IsTrue(PaperService.CanEdit(paper));
        }

        [TestMethod]
        public void CanEdit_NotLoggedIn_ReturnsFalse()
        {
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");

            Assert.IsNotNull(paper);
            Assert.IsFalse(PaperService.CanEdit(paper));
        }

        [TestMethod]
        public void EditTitle_WithPermission_ReturnsTrue()
        {
            AuthService.Login("fadiil@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");
            Assert.IsNotNull(paper);

            string originalTitle = paper.Title;
            bool result = PaperService.EditTitle(paper, "Updated Title");

            Assert.IsTrue(result);
            Assert.AreEqual("Updated Title", paper.Title);

            paper.Title = originalTitle;
        }

        [TestMethod]
        public void EditTitle_WithoutPermission_ReturnsFalse()
        {
            AuthService.Login("bacul@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");
            Assert.IsNotNull(paper);

            bool result = PaperService.EditTitle(paper, "Hacked Title");

            Assert.IsFalse(result);
        }

        [TestMethod]
        public void EditAbstract_WithPermission_ReturnsTrue()
        {
            AuthService.Login("fadiil@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");
            Assert.IsNotNull(paper);

            string originalAbstract = paper.Paper_Abstract;
            bool result = PaperService.EditAbstract(paper, "Updated Abstract");

            Assert.IsTrue(result);
            Assert.AreEqual("Updated Abstract", paper.Paper_Abstract);

            paper.Paper_Abstract = originalAbstract;
        }

        [TestMethod]
        public void EditAbstract_WithoutPermission_ReturnsFalse()
        {
            AuthService.Login("bacul@mail.com", "qwerty");
            Paper? paper = PaperService.SearchPaper("Lorem Ipsum: An Analysis");
            Assert.IsNotNull(paper);

            bool result = PaperService.EditAbstract(paper, "Hacked Abstract");

            Assert.IsFalse(result);
        }
    }

    [TestClass]
    public class SessionTest
    {
        [TestMethod]
        public void Session_IsSingleton()
        {
            var s1 = Session.Instance;
            var s2 = Session.Instance;

            Assert.AreSame(s1, s2);
        }

        [TestMethod]
        public void Logout_ClearsAccount()
        {
            AuthService.Login("radit@mail.com", "qwerty");
            Assert.IsNotNull(Session.Instance.Account);

            Session.Instance.Logout();

            Assert.IsNull(Session.Instance.Account);
            Assert.AreEqual(0, Session.Instance.Menu);
        }
    }

    [TestClass]
    public class DataContextTest
    {
        [TestMethod]
        public void Users_NotEmpty()
        {
            var users = DataContext.Users.GetAll();

            Assert.IsTrue(users.Any());
        }

        [TestMethod]
        public void Papers_NotEmpty()
        {
            var papers = DataContext.Papers.GetAll();

            Assert.IsTrue(papers.Any());
        }

        [TestMethod]
        public void Requests_NotEmpty()
        {
            var requests = DataContext.Requests.GetAll();

            Assert.IsTrue(requests.Any());
        }

        [TestMethod]
        public void Users_ContainsAdmin()
        {
            var admin = UserService.SearchUser("email", "admin@mail.com");

            Assert.IsNotNull(admin);
            Assert.AreEqual("admin", admin.Role);
        }
    }
}
