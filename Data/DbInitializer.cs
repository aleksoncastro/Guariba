using Guariba.Models;
using System;
using System.Linq;

namespace Guariba.Data
{
    public static class DbInitializer
    {
        public static void Initialize(SocialMediaContext context)
        {
            // --- Não usar EnsureCreated se estiver usando migrations ---
            // context.Database.EnsureCreated();

            // Se já tiver usuários, não faz nada
            if (context.          User.Any())
            {
                return;   // DB já populado
            }

            var now = DateTime.Now;

            // --- Criando usuários com PersonalInformation ---
            var users = new User[]
            {
                new User
                {
                    UserName="joao",
                    Email="joao@email.com",
                    PasswordHash="123", // Corrigido
                    RegistrationDat=now,
                    ProfilePhoto="/img/joao.png",
                    PersonalInformation = new PersonalInformation
                    {
                        FullName = "João Silva",
                        BirthDate = new DateTime(1990, 1, 1),
                        UserGender = Gender.MASCULINO
                    }
                },
                new User
                {
                    UserName="maria",
                    Email="maria@email.com",
                    PasswordHash="123", // Corrigido
                    RegistrationDat=now,
                    ProfilePhoto="/img/maria.png",
                    PersonalInformation = new PersonalInformation
                    {
                        FullName = "Maria Souza",
                        BirthDate = new DateTime(1992, 5, 10),
                        UserGender = Gender.FEMININO
                    }
                },
                new User
                {
                    UserName="ana",
                    Email="ana@email.com",
                    PasswordHash="123", // Corrigido
                    RegistrationDat=now,
                    ProfilePhoto="/img/ana.png",
                    PersonalInformation = new PersonalInformation
                    {
                        FullName = "Ana Oliveira",
                        BirthDate = new DateTime(1995, 3, 20),
                        UserGender = Gender.FEMININO
                    }
                },
                new User
                {
                    UserName="alek",
                    Email="alek@email.com",
                    PasswordHash="AQAAAAIAAYagAAAAENXFoTedp/kDMpOnX62/V/5W6fuFPvuZzYI12M9zJM7ap7gZq8SCMlcjR0QL7hGpow==", // Corrigido
                    RegistrationDat=now,
                    ProfilePhoto="/img/alek.png",
                    PersonalInformation = new PersonalInformation
                    {
                        FullName = "Alek Castro",
                        BirthDate = new DateTime(1990, 1, 1),
                        UserGender = Gender.MASCULINO
                    }
                },
            };

            context.User.AddRange(users);
            context.SaveChanges(); // salva User + PersonalInformation juntos

            // --- Criando posts ---
            var posts = new Post[]
            {
                new Post
                {
                    TextContent="Primeiro post do João!",
                    CreatedAt=now,
                    UserId=users[0].Id,
                    ImageUrl = "" // importante se ImageUrl não for nullable
                },
                new Post
                {
                    TextContent="Maria entrou na rede 🚀",
                    CreatedAt=now,
                    UserId=users[1].Id,
                    ImageUrl="/img/code.png"
                },
                new Post
                {
                    TextContent="Ana postando também :)",
                    CreatedAt=now,
                    UserId=users[2].Id,
                    ImageUrl = ""
                }
            };

            context.Post.AddRange(posts);
            context.SaveChanges();

            // --- Criando comentários ---
            var comments = new Comment[]
            {
                new Comment
                {
                    Text="Bem-vindo João!",
                    CreatedAt=now,
                    UserId=users[1].Id,
                    PostId=posts[0].Id
                },
                new Comment
                {
                    Text="Valeu Maria!",
                    CreatedAt=now,
                    UserId=users[0].Id,
                    PostId=posts[0].Id
                },
                new Comment
                {
                    Text="Boa Ana!",
                    CreatedAt=now,
                    UserId=users[0].Id,
                    PostId=posts[2].Id
                }
            };

            context.Comment.AddRange(comments);
            context.SaveChanges();
        }
    }
}
