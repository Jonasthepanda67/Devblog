USE master
GO

IF EXISTS (SELECT name FROM sys.databases WHERE name = 'Devblog')
    BEGIN
        ALTER DATABASE Devblog SET SINGLE_USER WITH ROLLBACK IMMEDIATE
        DROP DATABASE Devblog
    END
GO

CREATE DATABASE Devblog
GO

--------------------------------------------------------

USE Devblog
CREATE TABLE PersonTable 
(
Person_Id UniqueIdentifier NOT NULL PRIMARY KEY,
Fname nvarchar(80) NOT NULL,
Lname nvarchar(100) NOT NULL,
FullName nvarchar(181) NOT NULL,
UserName nvarchar(18) UNIQUE NOT NULL,
Age int NOT NULL,
Mail nvarchar(120) NOT NULL,
City nvarchar(70) NOT NULL,
Number nvarchar(30) NOT NULL,
Password nvarchar(200) NOT NULL,
CreationDate DATETIME NOT NULL DEFAULT CAST( GETDATE() AS DATE),
UserType nvarchar(10) NOT NULL DEFAULT 'User'
)

CREATE TABLE PostTable
(
Post_Id UniqueIdentifier NOT NULL PRIMARY KEY,
Title nvarchar(45) NOT NULL,
Reference nvarchar(200) NOT NULL,
Date DATETIME NOT NULL DEFAULT CAST( GETDATE() AS Date),
PostType nvarchar(15) NOT NULL,
IsDeleted Bit NOT NULL DEFAULT 0
)

CREATE TABLE BlogPostTable
(
BlogPost_Id INT IDENTITY NOT NULL PRIMARY KEY,
Post_Id UniqueIdentifier NOT NULL,
Weblog nvarchar(800) NOT NULL,
 FOREIGN KEY (Post_Id) REFERENCES PostTable(Post_Id)
)

CREATE TABLE ReviewTable
(
Review_Id INT IDENTITY NOT NULL PRIMARY KEY,
Post_Id UniqueIdentifier NOT NULL,
Pros nvarchar(300) NOT NULL,
Cons nvarchar(300) NOT NULL,
Stars int NOT NULL,
 FOREIGN KEY (Post_Id) REFERENCES PostTable(Post_Id)
)


CREATE TABLE ProjectTable
(
Project_Id INT IDENTITY NOT NULL PRIMARY KEY,
Post_Id UniqueIdentifier NOT NULL,
Description nvarchar(800) UNIQUE NOT NULL,
Image nvarchar(100) NOT NULL,
 FOREIGN KEY (Post_Id) REFERENCES PostTable(Post_Id)
)

CREATE TABLE TagsTable
(
Tag_Id UniqueIdentifier NOT NULL PRIMARY KEY,
Name nvarchar(100) NOT NULL
)

CREATE TABLE TagListTable
(
Tag_Id UniqueIdentifier NOT NULL,
Post_Id UniqueIdentifier NOT NULL,
 FOREIGN KEY (Tag_Id) REFERENCES TagsTable(Tag_Id),
 FOREIGN KEY (Post_Id) REFERENCES PostTable(Post_Id),
 CONSTRAINT PK_Taglist PRIMARY KEY (Tag_Id, Post_Id)
)
GO

CREATE TABLE CommentsTable
(
Comment_Id UNIQUEIDENTIFIER NOT NULL PRIMARY KEY,
Post_Id UNIQUEIDENTIFIER NOT NULL,
UserName nvarchar(18) NOT NULL,
Message nvarchar(500) NOT NULL,
CreationDate DATETIME NOT NULL DEFAULT CAST( GETDATE() AS Date),
FOREIGN KEY (Post_Id) REFERENCES PostTable(Post_Id),
FOREIGN KEY (UserName) REFERENCES PersonTable(UserName)
)

--------------------------------------------------------

INSERT INTO PersonTable
(Person_Id, Fname,Lname, FullName, UserName, Age, Mail, City, Number, Password, UserType)
VALUES
('37c13ea0-02f1-4bae-b598-620e0642e666', 'Jonas', 'Petersen', 'Jonas Petersen', 'Jonasthepanda', '19', 'jonasfpetersen1@gmail.com', 'Nordborg', '42766861', '7A28C3624CABCCE68EF5B0286133E61B4933DAA52E7FCE14F7CF68B6FD6F124F:6D2DCBA5567E3D087FC5F347B0637E9E:50000:SHA256', 'Author')
GO

INSERT INTO TagsTable
(Tag_Id, Name)
VALUES
('28E07F87-3727-43F4-A056-093E1FD884BB', 'Xaml'),
('8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F', 'Javascript'),
('B3E230E7-BDDC-4214-B725-1D28122E562D', 'Bootstrap'),
('774654F1-F836-437D-BF29-4E0D588EC698', 'MS SQL'),
('B3D3F1B6-D5A6-4172-9032-620CE226DC11', 'Python'),
('E941DB7B-FF70-45DC-803F-9A1FC10A35A3', 'Html'),
('9BB3BA7A-A3C8-466F-B746-AE6A7F9CB5B4', 'Css'),
('176D4216-841B-45A3-A664-CC4D24B1F669', 'WPF'),
('ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D', 'C#')
GO

INSERT INTO PostTable
(Post_Id, Title, Reference, Date, PostType)
VALUES
('51DE10F1-53A8-4984-AED0-A221AD5F8312', 'C#', 'https://dotnet.microsoft.com/en-us/languages/csharp', '2024-10-25', 'Review'),
('E3A5B207-CF44-4227-821A-731C7EC6CF57', 'Devblog', 'https://github.com/Jonasthepanda67/Devblog', '2024-9-30', 'Project'),
('a81529e8-a097-4228-b469-302fad194a3b', 'Blackjack', 'https://github.com/Jonasthepanda67/Blackjack', '2024-9-5', 'Project'),
('e5119f95-9c0f-423e-bdf0-88d12bc22f2e', 'The Bank', 'https://github.com/Jonasthepanda67/The-Bank', '2024-8-14', 'Project'),
('3c40f817-989c-4c38-8162-7268391e9be8', 'Snake Game', 'https://github.com/Jonasthepanda67/SnakeGame', '2024-9-23', 'Project'),
('3a836035-38a2-44d5-bd1f-f3ff808f5484', 'Devblog Update', 'https://github.com/Jonasthepanda67/Devblog/releases/tag/V2.0.0', '2024-10-11', 'BlogPost'),
('7a607db3-c163-4055-820f-e2d91786d706', 'Devblog Update', 'https://github.com/Jonasthepanda67/Devblog/releases/tag/V2.1.0', '2024-10-23', 'BlogPost'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', 'Devblog Update', 'https://github.com/Jonasthepanda67/Devblog/releases/tag/V2.2.0', '2024-10-23', 'BlogPost'),
('103e137d-d378-47f7-a1b1-1849b420fc17', 'Devblog Update', 'https://github.com/Jonasthepanda67/Devblog/releases/tag/V2.3.0', '2024-10-25', 'BlogPost')
GO

INSERT INTO BlogPostTable
(Post_Id, Weblog)
VALUES
('3a836035-38a2-44d5-bd1f-f3ff808f5484', 'Devblog Version 2.0.0 is out now!'),
('7a607db3-c163-4055-820f-e2d91786d706', 'Devblog Version 2.1.0 is out now!'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', 'Devblog Version 2.2.0 is out now!'),
('103e137d-d378-47f7-a1b1-1849b420fc17', 'Devblog Version 2.3.0 is out now!')
GO

INSERT INTO ProjectTable
(Post_Id, Description, Image)
VALUES
('E3A5B207-CF44-4227-821A-731C7EC6CF57', 'Devblog is a school project that i have worked on and plan on using in the future for all my projects. I plan on adding more and more features until its finish and at that point i will be using it for storing all my projects, reviews and updates.', '\Uploads\devblog project.png'),
('a81529e8-a097-4228-b469-302fad194a3b', 'This is a school project where we had to create the game Blackjack also called 21. I made the version of blackjack that i have played before with the rules i think makes most sense.', '\Uploads\Blackjack project.png'),
('e5119f95-9c0f-423e-bdf0-88d12bc22f2e', 'A simple bank application with features like multiple types of accounts and deposit or withdraw functions.', '\Uploads\The Bank project.png'),
('3c40f817-989c-4c38-8162-7268391e9be8', 'Welcome to SnakeGame, a project developed for a school assignment. In this take on the Snake game, you control a growing snake, collecting fruits and avoiding collisions with yourself or the walls. I’ve put my own spin on it with unique features like multiple difficulty levels, custom designs, and even a nightmare mode for the brave souls that dont know any better!', '\Uploads\Snake Game project.png')
GO

INSERT INTO ReviewTable
(Post_Id, Pros, Cons, Stars)
VALUES
('51DE10F1-53A8-4984-AED0-A221AD5F8312', 'Very beginner friendly and in general nice to use. It is very easy to find help on the internet for C# if needed and a lot of people make super useful plugins and packages for it.', 'It is limited to Microsoft Platforms. It is less suitable for making highly optimized applications on low power devices', 5)
GO

INSERT INTO TagListTable
(Post_Id, Tag_Id)
VALUES
('E3A5B207-CF44-4227-821A-731C7EC6CF57', '8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F'),
('E3A5B207-CF44-4227-821A-731C7EC6CF57', 'B3E230E7-BDDC-4214-B725-1D28122E562D'),
('E3A5B207-CF44-4227-821A-731C7EC6CF57', '774654F1-F836-437D-BF29-4E0D588EC698'),
('E3A5B207-CF44-4227-821A-731C7EC6CF57', 'E941DB7B-FF70-45DC-803F-9A1FC10A35A3'),
('E3A5B207-CF44-4227-821A-731C7EC6CF57', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('3a836035-38a2-44d5-bd1f-f3ff808f5484', '8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F'),
('3a836035-38a2-44d5-bd1f-f3ff808f5484', 'B3E230E7-BDDC-4214-B725-1D28122E562D'),
('3a836035-38a2-44d5-bd1f-f3ff808f5484', '774654F1-F836-437D-BF29-4E0D588EC698'),
('3a836035-38a2-44d5-bd1f-f3ff808f5484', 'E941DB7B-FF70-45DC-803F-9A1FC10A35A3'),
('3a836035-38a2-44d5-bd1f-f3ff808f5484', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('7a607db3-c163-4055-820f-e2d91786d706', '8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F'),
('7a607db3-c163-4055-820f-e2d91786d706', 'B3E230E7-BDDC-4214-B725-1D28122E562D'),
('7a607db3-c163-4055-820f-e2d91786d706', '774654F1-F836-437D-BF29-4E0D588EC698'),
('7a607db3-c163-4055-820f-e2d91786d706', 'E941DB7B-FF70-45DC-803F-9A1FC10A35A3'),
('7a607db3-c163-4055-820f-e2d91786d706', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', '8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', 'B3E230E7-BDDC-4214-B725-1D28122E562D'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', '774654F1-F836-437D-BF29-4E0D588EC698'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', 'E941DB7B-FF70-45DC-803F-9A1FC10A35A3'),
('e9f7573c-f027-44ac-a401-574b65dc0f08', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('103e137d-d378-47f7-a1b1-1849b420fc17', '8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F'),
('103e137d-d378-47f7-a1b1-1849b420fc17', 'B3E230E7-BDDC-4214-B725-1D28122E562D'),
('103e137d-d378-47f7-a1b1-1849b420fc17', '774654F1-F836-437D-BF29-4E0D588EC698'),
('103e137d-d378-47f7-a1b1-1849b420fc17', 'E941DB7B-FF70-45DC-803F-9A1FC10A35A3'),
('103e137d-d378-47f7-a1b1-1849b420fc17', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('51DE10F1-53A8-4984-AED0-A221AD5F8312', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('a81529e8-a097-4228-b469-302fad194a3b', '8E69E15B-EEF7-4FBF-A4FB-0D8EA664C72F'),
('a81529e8-a097-4228-b469-302fad194a3b', 'B3E230E7-BDDC-4214-B725-1D28122E562D'),
('a81529e8-a097-4228-b469-302fad194a3b', 'E941DB7B-FF70-45DC-803F-9A1FC10A35A3'),
('a81529e8-a097-4228-b469-302fad194a3b', '9BB3BA7A-A3C8-466F-B746-AE6A7F9CB5B4'),
('e5119f95-9c0f-423e-bdf0-88d12bc22f2e', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('3c40f817-989c-4c38-8162-7268391e9be8', 'ACDF17C8-6BE2-4FF9-82ED-D35EDEBF803D'),
('3c40f817-989c-4c38-8162-7268391e9be8', '176D4216-841B-45A3-A664-CC4D24B1F669'),
('3c40f817-989c-4c38-8162-7268391e9be8', '28E07F87-3727-43F4-A056-093E1FD884BB')
GO

-------------------------------------------------------

CREATE PROCEDURE sp_CreateReview
@Post_Id nvarchar(50),
@Title nvarchar(45),
@Reference nvarchar(200),
@PostType nvarchar(15),
@Pros nvarchar(300),
@Cons nvarchar(300),
@Stars int
AS
BEGIN
    INSERT INTO PostTable(Post_Id, Title, Reference, PostType) VALUES (@Post_Id, @Title, @Reference, @PostType)
    INSERT INTO ReviewTable(Post_Id, Pros, Cons, Stars) VALUES (@Post_Id, @Pros, @Cons, @Stars)
END
GO

CREATE PROCEDURE sp_CreateBlogPost
@Post_Id nvarchar(50),
@Title nvarchar(45),
@Reference nvarchar(200),
@PostType nvarchar(15),
@Weblog nvarchar(800)
AS
BEGIN
    INSERT INTO PostTable(Post_Id, Title, Reference, PostType) VALUES (@Post_Id, @Title, @Reference, @PostType)
    INSERT INTO BlogPostTable(Post_Id, Weblog) VALUES (@Post_Id, @Weblog)
END
GO

CREATE PROCEDURE sp_CreateProject
@Post_Id nvarchar(50),
@Title nvarchar(45),
@Reference nvarchar(200),
@PostType nvarchar(15),
@Description nvarchar(800),
@Image nvarchar(100)
AS
BEGIN
    INSERT INTO PostTable(Post_Id, Title, Reference, PostType) VALUES (@Post_Id, @Title, @Reference, @PostType)
    INSERT INTO ProjectTable(Post_Id, Description, Image) VALUES (@Post_Id, @Description, @Image)
END
GO

CREATE PROCEDURE sp_SoftDelete
@Post_Id nvarchar(50)
AS
BEGIN
    UPDATE PostTable
    SET IsDeleted = 1
    WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_RestorePost
@Post_Id nvarchar(50)
AS
BEGIN
    UPDATE PostTable
    SET IsDeleted = 0
    WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_FullDeleteBlogPost
@Post_Id nvarchar(50)
AS
BEGIN
    DELETE FROM TagListTable WHERE Post_Id = @Post_Id
    DELETE FROM BlogPostTable WHERE Post_Id = @Post_Id
    DELETE FROM PostTable WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_FullDeleteReview
@Post_Id nvarchar(50)
AS
BEGIN
    DELETE FROM TagListTable WHERE Post_Id = @Post_Id
    DELETE FROM ReviewTable WHERE Post_Id = @Post_Id
    DELETE FROM PostTable WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_FullDeleteProject
@Post_Id nvarchar(50)
AS
BEGIN
    DELETE FROM TagListTable WHERE Post_Id = @Post_Id
    DELETE FROM ProjectTable WHERE Post_Id = @Post_Id
    DELETE FROM PostTable WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_UpdateReview
@Post_Id nvarchar(50),
@Title nvarchar(45),
@Reference nvarchar(200),
@Pros nvarchar(300),
@Cons nvarchar(300),
@Stars int
AS
BEGIN
    UPDATE PostTable
    SET Title = @Title, Reference = @Reference
    WHERE Post_Id = @Post_Id
    UPDATE ReviewTable
    SET Pros = @Pros, Cons = @Cons, Stars = @Stars
    WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_UpdateBlogPost
@Post_Id nvarchar(50),
@Title nvarchar(45),
@Reference nvarchar(200),
@Weblog nvarchar(800)
AS
BEGIN
    UPDATE PostTable
    SET Title = @Title, Reference = @Reference
    WHERE Post_Id = @Post_Id
    UPDATE BlogPostTable
    SET Weblog = @Weblog
    WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_UpdateProject
@Post_Id nvarchar(50),
@Title nvarchar(45),
@Reference nvarchar(200),
@Description nvarchar(800),
@Image nvarchar(100)
AS
BEGIN
    UPDATE PostTable
    SET Title = @Title, Reference = @Reference
    WHERE Post_Id = @Post_Id
    UPDATE ProjectTable
    SET Description = @Description, Image = @Image
    WHERE Post_Id = @Post_Id
END
GO

CREATE PROCEDURE sp_CreateTag
@Tag_Id nvarchar(50),
@Name nvarchar(100)
AS
INSERT INTO TagsTable(Tag_Id, Name) VALUES (@Tag_Id, @Name)
GO

CREATE PROCEDURE sp_DeleteTag
@Tag_Id nvarchar(50)
AS
DELETE FROM TagListTable WHERE Tag_Id = @Tag_Id
DELETE FROM TagsTable WHERE Tag_Id = @Tag_Id
GO

CREATE PROCEDURE sp_UpdateTag
@Tag_Id nvarchar(50),
@Name nvarchar(100)
AS
BEGIN
	UPDATE TagsTable
    SET Name = @Name
    WHERE Tag_Id = @Tag_Id
END
GO

CREATE PROCEDURE sp_AddTagToPost
@Post_Id nvarchar(50),
@Tag_Id nvarchar(50)
AS
INSERT INTO TagListTable(Tag_Id, Post_Id) VALUES (@Tag_Id, @Post_Id)
GO

CREATE PROCEDURE sp_RemoveTagFromPost
@Tag_Id nvarchar(50),
@Post_Id nvarchar(50)
AS
DELETE FROM TagListTable WHERE Tag_Id = @Tag_Id AND Post_Id = @Post_Id
GO

CREATE PROCEDURE sp_GetTagsForPost
@Post_Id nvarchar(50)
AS
SELECT *
FROM
     TagListTable
INNER JOIN
     TagsTable ON TagListTable.Tag_Id = TagsTable.Tag_Id
 WHERE
     TagListTable.Post_Id = @Post_Id
GO

CREATE PROCEDURE sp_CreateUserAccount
@Person_Id nvarchar(50),
@FName nvarchar(80),
@Lname nvarchar(100),
@FullName nvarchar(181),
@UserName nvarchar(18),
@Age int,
@Mail nvarchar(120),
@City nvarchar(70),
@Number nvarchar(30),
@Password nvarchar(200),
@CreationDate DATETIME
AS
INSERT INTO PersonTable(Person_Id, Fname, Lname, FullName, UserName, Age, Mail, City, Number, Password, CreationDate) VALUES (@Person_Id, @FName, @Lname, @FullName, @UserName, @Age, @Mail, @City, @Number, @Password, @CreationDate)
GO

CREATE PROCEDURE sp_DeleteUserAccount
@Person_Id nvarchar(50)
AS
DELETE FROM PersonTable WHERE Person_Id = @Person_Id
GO

CREATE PROCEDURE sp_UpdateUserAccount
@Person_Id nvarchar(50),
@FName nvarchar(80),
@Lname nvarchar(100),
@FullName nvarchar(181),
@Age int,
@Mail nvarchar(120),
@City nvarchar(70),
@Number nvarchar(30),
@Password nvarchar(200),
@UserType nvarchar(10)
AS
BEGIN
	UPDATE PersonTable
    SET Fname = @FName, Lname = @Lname, FullName = @FullName, Age = @Age, Mail = @Mail, City = @City, Number = @Number, Password = @Password, UserType = @UserType
    WHERE Person_Id = @Person_Id
END
GO

CREATE PROCEDURE sp_CreateComment
@Comment_Id nvarchar(50),
@Post_Id nvarchar(50),
@UserName nvarchar(18),
@Message nvarchar(500),
@CreationDate DATETIME
AS
INSERT INTO CommentsTable(Comment_Id, Post_Id, UserName, Message, CreationDate) VALUES (@Comment_Id, @Post_Id, @UserName, @Message, @CreationDate)
GO

CREATE PROCEDURE sp_DeleteComment
@Comment_Id nvarchar(50)
AS
DELETE FROM CommentsTable WHERE Comment_Id = @Comment_Id
GO

CREATE PROCEDURE sp_UpdateComment
@Comment_Id nvarchar(50),
@Message nvarchar(500)
AS
BEGIN
UPDATE CommentsTable
SET Message = @Message
WHERE Comment_Id = @Comment_Id
END
GO

------------------------------------------------------

CREATE VIEW vw_PortfolioView
AS
SELECT 
    p.Title,
    p.Reference,
    p.Date,
    p.PostType,
    p.IsDeleted,
    pr.Description,
    pr.Image
FROM 
    PostTable p
INNER JOIN 
    ProjectTable pr ON p.Post_Id = pr.Post_Id
WHERE 
    p.PostType = 'Project'
GO

CREATE VIEW vw_ReviewBlogPostView
AS
SELECT 
    p.Title,
    p.Reference,
    p.Date,
    p.PostType,
    p.IsDeleted,
    bp.Weblog,
    r.Pros,
    r.Cons,
    r.Stars
FROM 
    PostTable p
LEFT JOIN 
    BlogPostTable bp ON p.Post_Id = bp.Post_Id
LEFT JOIN 
    ReviewTable r ON p.Post_Id = r.Post_Id
WHERE 
    p.PostType IN ('BlogPost', 'Review')
GO

------------------------------------------------------
-- Index for faster querying on Post_Id
CREATE UNIQUE INDEX UIX_PostTable_Post_Id ON PostTable (Post_Id);
CREATE UNIQUE INDEX UIX_BlogPostTable_Post_Id ON BlogPostTable (Post_Id);
CREATE UNIQUE INDEX UIX_ReviewTable_Post_Id ON ReviewTable (Post_Id);
CREATE UNIQUE INDEX UIX_ProjectTable_Post_Id ON ProjectTable (Post_Id);
CREATE INDEX UIX_TagListTable_Post_Id ON TagListTable (Post_Id);

-- Index for Tag_Id in TagListTable
CREATE INDEX IX_TagListTable_Tag_Id ON TagListTable (Tag_Id);

-- Index for Mail in PersonTable
CREATE INDEX IX_PersonTable_Mail ON PersonTable (Mail);
------------------------------------------------------
