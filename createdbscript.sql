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
Person_Id UniqueIdentifier NOT NULL,
Fname nvarchar(80) NOT NULL,
Lname nvarchar(100) NOT NULL,
FullName nvarchar(181) NOT NULL,
Age int NOT NULL,
Mail nvarchar(120) NOT NULL,
City nvarchar(70) NOT NULL,
Number nvarchar(30) NOT NULL,
Password nvarchar(200) NOT NULL,
CreationDate DATETIME NOT NULL DEFAULT CAST( GETDATE() AS DATE),
IsAuthor bit NOT NULL DEFAULT 0
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
Description nvarchar(800) NOT NULL,
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

--------------------------------------------------------

INSERT INTO PersonTable
(Person_Id, Fname,Lname, FullName, Age, Mail, City, Number, Password, IsAuthor)
VALUES

('37c13ea0-02f1-4bae-b598-620e0642e666', 'Jonas', 'Petersen', 'Jonas Petersen', '19', 'jonasfpetersen1@gmail.com', 'Nordborg', '+4542766861', 'password123', 1)
GO

INSERT INTO PostTable
(Post_Id, Title, Reference, Date, PostType)
VALUES

('E74750C1-5965-4816-82A2-1F30D70DE95F', 'Devblog Update', 'devblogupdates.dk', '2024-10-10 00:00:00.000', 'BlogPost')
GO

INSERT INTO PostTable
(Post_Id, Title, Reference, Date, PostType)
VALUES

('51DE10F1-53A8-4984-AED0-A221AD5F8312', 'Javascript', 'javasacrifice.com', '2024-10-10 00:00:00.000', 'Review')
GO

INSERT INTO PostTable
(Post_Id, Title, Reference, Date, PostType)
VALUES

('F6873D1F-9A88-46BB-9552-C878FCE4DC30', 'Biblioteket', 'bibliotekproject.dk', '2024-10-10 00:00:00.000', 'Project')
GO

INSERT INTO BlogPostTable
(Post_Id, Weblog)
VALUES

('E74750C1-5965-4816-82A2-1F30D70DE95F', 'Hotfix update is out now!')
GO

INSERT INTO ProjectTable
(Post_Id, Description, Image)
VALUES

('F6873D1F-9A88-46BB-9552-C878FCE4DC30', 'Skole project omkring et bibliotek', 'dgjdigjdigj')
GO

INSERT INTO ReviewTable
(Post_Id, Pros, Cons, Stars)
VALUES

('51DE10F1-53A8-4984-AED0-A221AD5F8312', 'Very nice for styling and implementing custom visual effects', 'idk', 4)
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
@Age int,
@Mail nvarchar(120),
@City nvarchar(70),
@Number nvarchar(30),
@Password nvarchar(200),
@CreationDate DATETIME
AS
INSERT INTO PersonTable(Person_Id, Fname, Lname, FullName, Age, Mail, City, Number, Password, CreationDate) VALUES (@Person_Id, @FName, @Lname, @FullName, @Age, @Mail, @City, @Number, @Password, @CreationDate)
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
@Password nvarchar(200)
AS
BEGIN
	UPDATE PersonTable
    SET Fname = @FName, Lname = @Lname, FullName = @FullName, Age = @Age, Mail = @Mail, City = @City, Number = @Number, Password = @Password
    WHERE Person_Id = @Person_Id
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
CREATE UNIQUE INDEX IX_PostTable_Post_Id ON PostTable (Post_Id);
CREATE UNIQUE INDEX IX_BlogPostTable_Post_Id ON BlogPostTable (Post_Id);
CREATE UNIQUE INDEX IX_ReviewTable_Post_Id ON ReviewTable (Post_Id);
CREATE UNIQUE INDEX IX_ProjectTable_Post_Id ON ProjectTable (Post_Id);
CREATE UNIQUE INDEX IX_TagListTable_Post_Id ON TagListTable (Post_Id);

-- Index for Tag_Id in TagListTable
CREATE INDEX IX_TagListTable_Tag_Id ON TagListTable (Tag_Id);

-- Index for Mail in PersonTable
CREATE INDEX IX_PersonTable_Mail ON PersonTable (Mail);

------------------------------------------------------
