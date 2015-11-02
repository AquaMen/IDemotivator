
-- --------------------------------------------------
-- Entity Designer DDL Script for SQL Server 2005, 2008, 2012 and Azure
-- --------------------------------------------------
-- Date Created: 11/02/2015 16:26:28
-- Generated from EDMX file: D:\Itransition\GitInt\IDemotivator\IDemotivator\Model.edmx
-- --------------------------------------------------

SET QUOTED_IDENTIFIER OFF;
GO
USE [aspnet-IDemotivator-20151024111336];
GO
IF SCHEMA_ID(N'dbo') IS NULL EXECUTE(N'CREATE SCHEMA [dbo]');
GO

-- --------------------------------------------------
-- Dropping existing FOREIGN KEY constraints
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[FK_AspNetUserDemotivator]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Demotivators] DROP CONSTRAINT [FK_AspNetUserDemotivator];
GO
IF OBJECT_ID(N'[dbo].[FK_tagtag_to_dem]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tag_to_dem] DROP CONSTRAINT [FK_tagtag_to_dem];
GO
IF OBJECT_ID(N'[dbo].[FK_tag_to_demDemotivator]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[tag_to_dem] DROP CONSTRAINT [FK_tag_to_demDemotivator];
GO
IF OBJECT_ID(N'[dbo].[FK_Demotivatorrate]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[rates] DROP CONSTRAINT [FK_Demotivatorrate];
GO
IF OBJECT_ID(N'[dbo].[FK_rateAspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[rates] DROP CONSTRAINT [FK_rateAspNetUser];
GO
IF OBJECT_ID(N'[dbo].[FK_DemotivatorComment]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_DemotivatorComment];
GO
IF OBJECT_ID(N'[dbo].[FK_CommentAspNetUser]', 'F') IS NOT NULL
    ALTER TABLE [dbo].[Comments] DROP CONSTRAINT [FK_CommentAspNetUser];
GO

-- --------------------------------------------------
-- Dropping existing tables
-- --------------------------------------------------

IF OBJECT_ID(N'[dbo].[Demotivators]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Demotivators];
GO
IF OBJECT_ID(N'[dbo].[tags]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tags];
GO
IF OBJECT_ID(N'[dbo].[tag_to_dem]', 'U') IS NOT NULL
    DROP TABLE [dbo].[tag_to_dem];
GO
IF OBJECT_ID(N'[dbo].[rates]', 'U') IS NOT NULL
    DROP TABLE [dbo].[rates];
GO
IF OBJECT_ID(N'[dbo].[Comments]', 'U') IS NOT NULL
    DROP TABLE [dbo].[Comments];
GO


-- --------------------------------------------------
-- Creating all tables
-- --------------------------------------------------

-- Creating table 'C__MigrationHistory'
CREATE TABLE [dbo].[C__MigrationHistory] (
    [MigrationId] nvarchar(150)  NOT NULL,
    [ContextKey] nvarchar(300)  NOT NULL,
    [Model] varbinary(max)  NOT NULL,
    [ProductVersion] nvarchar(32)  NOT NULL
);
GO

-- Creating table 'AspNetRoles'
CREATE TABLE [dbo].[AspNetRoles] (
    [Id] nvarchar(128)  NOT NULL,
    [Name] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'AspNetUserClaims'
CREATE TABLE [dbo].[AspNetUserClaims] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [UserId] nvarchar(128)  NOT NULL,
    [ClaimType] nvarchar(max)  NULL,
    [ClaimValue] nvarchar(max)  NULL
);
GO

-- Creating table 'AspNetUserLogins'
CREATE TABLE [dbo].[AspNetUserLogins] (
    [LoginProvider] nvarchar(128)  NOT NULL,
    [ProviderKey] nvarchar(128)  NOT NULL,
    [UserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUsers'
CREATE TABLE [dbo].[AspNetUsers] (
    [Id] nvarchar(128)  NOT NULL,
    [Email] nvarchar(256)  NULL,
    [EmailConfirmed] bit  NOT NULL,
    [PasswordHash] nvarchar(max)  NULL,
    [SecurityStamp] nvarchar(max)  NULL,
    [PhoneNumber] nvarchar(max)  NULL,
    [PhoneNumberConfirmed] bit  NOT NULL,
    [TwoFactorEnabled] bit  NOT NULL,
    [LockoutEndDateUtc] datetime  NULL,
    [LockoutEnabled] bit  NOT NULL,
    [AccessFailedCount] int  NOT NULL,
    [UserName] nvarchar(256)  NOT NULL
);
GO

-- Creating table 'Demotivators'
CREATE TABLE [dbo].[Demotivators] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [AspNetUserId] nvarchar(128)  NOT NULL,
    [Name] nvarchar(max)  NULL,
    [Date] datetime  NOT NULL,
    [Url_Img] nvarchar(max)  NOT NULL,
    [Url_Img_Origin] nvarchar(max)  NOT NULL,
    [Str1] nvarchar(max)  NOT NULL,
    [Str2] nvarchar(max)  NOT NULL,
    [Rate] nvarchar(max)  NULL,
    [JSON] nvarchar(max)  NOT NULL
);
GO

-- Creating table 'tags'
CREATE TABLE [dbo].[tags] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Name] nvarchar(max)  NOT NULL,
    [Count] int  NOT NULL
);
GO

-- Creating table 'tag_to_dem'
CREATE TABLE [dbo].[tag_to_dem] (
    [tagId] int  NOT NULL,
    [DemotivatorId] int  NOT NULL,
    [Id] int IDENTITY(1,1) NOT NULL
);
GO

-- Creating table 'rates'
CREATE TABLE [dbo].[rates] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [DemotivatorId] int  NOT NULL,
    [AspNetUserId] nvarchar(128)  NOT NULL,
    [Count] int  NULL,
    [IsRate] bit  NOT NULL
);
GO

-- Creating table 'Comments'
CREATE TABLE [dbo].[Comments] (
    [Id] int IDENTITY(1,1) NOT NULL,
    [Text] nvarchar(max)  NOT NULL,
    [Date] datetime  NOT NULL,
    [DemotivatorId] int  NOT NULL,
    [AspNetUserId] nvarchar(128)  NOT NULL
);
GO

-- Creating table 'AspNetUserRoles'
CREATE TABLE [dbo].[AspNetUserRoles] (
    [AspNetRoles_Id] nvarchar(128)  NOT NULL,
    [AspNetUsers_Id] nvarchar(128)  NOT NULL
);
GO

-- --------------------------------------------------
-- Creating all PRIMARY KEY constraints
-- --------------------------------------------------

-- Creating primary key on [MigrationId], [ContextKey] in table 'C__MigrationHistory'
ALTER TABLE [dbo].[C__MigrationHistory]
ADD CONSTRAINT [PK_C__MigrationHistory]
    PRIMARY KEY CLUSTERED ([MigrationId], [ContextKey] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetRoles'
ALTER TABLE [dbo].[AspNetRoles]
ADD CONSTRAINT [PK_AspNetRoles]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [PK_AspNetUserClaims]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [LoginProvider], [ProviderKey], [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [PK_AspNetUserLogins]
    PRIMARY KEY CLUSTERED ([LoginProvider], [ProviderKey], [UserId] ASC);
GO

-- Creating primary key on [Id] in table 'AspNetUsers'
ALTER TABLE [dbo].[AspNetUsers]
ADD CONSTRAINT [PK_AspNetUsers]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Demotivators'
ALTER TABLE [dbo].[Demotivators]
ADD CONSTRAINT [PK_Demotivators]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tags'
ALTER TABLE [dbo].[tags]
ADD CONSTRAINT [PK_tags]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'tag_to_dem'
ALTER TABLE [dbo].[tag_to_dem]
ADD CONSTRAINT [PK_tag_to_dem]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'rates'
ALTER TABLE [dbo].[rates]
ADD CONSTRAINT [PK_rates]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [Id] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [PK_Comments]
    PRIMARY KEY CLUSTERED ([Id] ASC);
GO

-- Creating primary key on [AspNetRoles_Id], [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [PK_AspNetUserRoles]
    PRIMARY KEY CLUSTERED ([AspNetRoles_Id], [AspNetUsers_Id] ASC);
GO

-- --------------------------------------------------
-- Creating all FOREIGN KEY constraints
-- --------------------------------------------------

-- Creating foreign key on [UserId] in table 'AspNetUserClaims'
ALTER TABLE [dbo].[AspNetUserClaims]
ADD CONSTRAINT [FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserClaims_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserClaims]
    ([UserId]);
GO

-- Creating foreign key on [UserId] in table 'AspNetUserLogins'
ALTER TABLE [dbo].[AspNetUserLogins]
ADD CONSTRAINT [FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
    FOREIGN KEY ([UserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE CASCADE ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId'
CREATE INDEX [IX_FK_dbo_AspNetUserLogins_dbo_AspNetUsers_UserId]
ON [dbo].[AspNetUserLogins]
    ([UserId]);
GO

-- Creating foreign key on [AspNetRoles_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetRoles]
    FOREIGN KEY ([AspNetRoles_Id])
    REFERENCES [dbo].[AspNetRoles]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating foreign key on [AspNetUsers_Id] in table 'AspNetUserRoles'
ALTER TABLE [dbo].[AspNetUserRoles]
ADD CONSTRAINT [FK_AspNetUserRoles_AspNetUsers]
    FOREIGN KEY ([AspNetUsers_Id])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserRoles_AspNetUsers'
CREATE INDEX [IX_FK_AspNetUserRoles_AspNetUsers]
ON [dbo].[AspNetUserRoles]
    ([AspNetUsers_Id]);
GO

-- Creating foreign key on [AspNetUserId] in table 'Demotivators'
ALTER TABLE [dbo].[Demotivators]
ADD CONSTRAINT [FK_AspNetUserDemotivator]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_AspNetUserDemotivator'
CREATE INDEX [IX_FK_AspNetUserDemotivator]
ON [dbo].[Demotivators]
    ([AspNetUserId]);
GO

-- Creating foreign key on [tagId] in table 'tag_to_dem'
ALTER TABLE [dbo].[tag_to_dem]
ADD CONSTRAINT [FK_tagtag_to_dem]
    FOREIGN KEY ([tagId])
    REFERENCES [dbo].[tags]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tagtag_to_dem'
CREATE INDEX [IX_FK_tagtag_to_dem]
ON [dbo].[tag_to_dem]
    ([tagId]);
GO

-- Creating foreign key on [DemotivatorId] in table 'tag_to_dem'
ALTER TABLE [dbo].[tag_to_dem]
ADD CONSTRAINT [FK_tag_to_demDemotivator]
    FOREIGN KEY ([DemotivatorId])
    REFERENCES [dbo].[Demotivators]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_tag_to_demDemotivator'
CREATE INDEX [IX_FK_tag_to_demDemotivator]
ON [dbo].[tag_to_dem]
    ([DemotivatorId]);
GO

-- Creating foreign key on [DemotivatorId] in table 'rates'
ALTER TABLE [dbo].[rates]
ADD CONSTRAINT [FK_Demotivatorrate]
    FOREIGN KEY ([DemotivatorId])
    REFERENCES [dbo].[Demotivators]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_Demotivatorrate'
CREATE INDEX [IX_FK_Demotivatorrate]
ON [dbo].[rates]
    ([DemotivatorId]);
GO

-- Creating foreign key on [AspNetUserId] in table 'rates'
ALTER TABLE [dbo].[rates]
ADD CONSTRAINT [FK_rateAspNetUser]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_rateAspNetUser'
CREATE INDEX [IX_FK_rateAspNetUser]
ON [dbo].[rates]
    ([AspNetUserId]);
GO

-- Creating foreign key on [DemotivatorId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_DemotivatorComment]
    FOREIGN KEY ([DemotivatorId])
    REFERENCES [dbo].[Demotivators]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_DemotivatorComment'
CREATE INDEX [IX_FK_DemotivatorComment]
ON [dbo].[Comments]
    ([DemotivatorId]);
GO

-- Creating foreign key on [AspNetUserId] in table 'Comments'
ALTER TABLE [dbo].[Comments]
ADD CONSTRAINT [FK_CommentAspNetUser]
    FOREIGN KEY ([AspNetUserId])
    REFERENCES [dbo].[AspNetUsers]
        ([Id])
    ON DELETE NO ACTION ON UPDATE NO ACTION;
GO

-- Creating non-clustered index for FOREIGN KEY 'FK_CommentAspNetUser'
CREATE INDEX [IX_FK_CommentAspNetUser]
ON [dbo].[Comments]
    ([AspNetUserId]);
GO

-- --------------------------------------------------
-- Script has ended
-- --------------------------------------------------