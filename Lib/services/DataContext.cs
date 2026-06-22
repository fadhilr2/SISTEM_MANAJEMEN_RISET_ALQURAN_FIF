using Lib.models;
using Lib.services;

namespace Lib.services
{
    public static class DataContext
    {
        private static readonly User[] USERS =
        {
            new User
            {
                Name = "Raditya Atallahasyrif Rachmadie",
                Email = "radit@mail.com",
                Password = "qwerty",
                Role = "researcher",
            },
            new User
            {
                Name = "Fadiil Rizky Akbar",
                Email = "fadiil@mail.com",
                Password = "qwerty",
                Role = "researcher",
            },
            new User
            {
                Name = "Muhammad Rasul",
                Email = "bacul@mail.com",
                Password = "qwerty",
                Role = "visitor",
            },
            new User
            {
                Name = "Muhammad Farhan Abdillah",
                Email = "farhan@mail.com",
                Password = "qwerty",
                Role = "visitor",
            },
            new User
            {
                Name = "I Putu Gede Deva Gundhala",
                Email = "deva@mail.com",
                Password = "qwerty",
                Role = "visitor",
            },
            new User
            {
                Name = "Admin",
                Email = "admin@mail.com",
                Password = "qwerty",
                Role = "admin",
            },
        };

        private static readonly Paper[] PAPERS =
        {
            new Paper
            {
                Title = "Lorem Ipsum: An Analysis",
                Paper_Abstract = "Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet Lorem Ipsum Dolor Sit Amet",
                Author = "Fadiil Rizky Akbar",
                Date = "June 13th 2026",
            },
            new Paper
            {
                Title = "Quantum Computing in Modern Cryptography",
                Paper_Abstract = "This paper explores the implications of quantum computing on modern cryptographic algorithms, focusing on post-quantum security measures and the vulnerabilities of RSA encryption.",
                Author = "Dr. Jane Smith",
                Date = "January 15th 2026",
            },
            new Paper
            {
                Title = "The Rise of Autonomous Systems in Agriculture",
                Paper_Abstract = "An in-depth study on how automated drones and AI-driven tractors are reshaping efficiency metrics in large-scale farming operations, balancing yield increases with initial capital expenditure.",
                Author = "Dr. Alan Turing",
                Date = "March 22nd 2026",
            },
            new Paper
            {
                Title = "Microplastics and Marine Ecosystems: A Ten-Year Review",
                Paper_Abstract = "Aggregating a decade of environmental data, this review highlights the escalating concentration of microplastics in deep-sea organisms and proposes new international policy frameworks.",
                Author = "Professor Elena Rostova",
                Date = "November 5th 2025",
            },
            new Paper
            {
                Title = "Architectural Patterns in Distributed Microservices",
                Paper_Abstract = "A comparative analysis of event-driven versus RESTful architectures in high-throughput enterprise applications, evaluating latency, fault tolerance, and developer velocity.",
                Author = "Linus Torvalds",
                Date = "May 19th 2026",
            }
        };

        private static readonly User[] REQUESTS =
        {
            new User
            {
                Name = "Bacon",
                Email = "bacon@mail.com",
                Password = "qwerty",
                Role = "visitor",
            }
        };

        public static Repository<User> Users { get; } = new(USERS);
        public static Repository<Paper> Papers { get; } = new(PAPERS);
        public static Repository<User> Requests { get; } = new(REQUESTS);
    }
}
