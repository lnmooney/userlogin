CREATE PROCEDURE [dbo].[Validate_User]
      @Username NVARCHAR(20),
      @Password NVARCHAR(20)
AS
BEGIN
      SET NOCOUNT ON;
      DECLARE @UserId INT
     
      SELECT @UserId = UserId
      FROM Users WHERE Username = @Username AND [Password] = @Password
END

INSERT INTO dbo.Users (Username, Password)
    VALUES ('lnmooney', 'password1!');

SELECT * FROM dbo.Users;