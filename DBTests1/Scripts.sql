
/* Get max ID of change where content is not null 
 Create a new record (insert) with Id max + 1, and our instanceID, content null . Save ID as current chnage record > If we get an error, it's because someone else beat us to it. Abort
 Get busy getting all the previous chnages we haven't applied yet.
 Apply them to our local copy.
 Generate a difference
 Modify (update) record id current change record. Check that instance ID matches ours (might not be necessary since in theory we're the only ones modifying this record. Make sure previous content was null again maybe not necessary)
 Now that content is no longer null, this chnage record will show in the initial query and the next number can be claimed !
 House keeping: If an Instance claims the current change and never updates the content, we'll be blocked. We need a timeout of some sort.

 Maybe we can simply have code that runs when someone tries to claim the current change: if current chnage record (max id with content null) creation time <> Now larger than XX, then delete it.

 */

 /* OpenChangeOperation(instanceID) returns seqno of new change operation (update to change seqno -1)
	Also handles case where the current chnage record has been help too long.

	CompleteAndCloseChange(seqno, instanceID, content) return ok (0) or error (-1)  */

if OBJECT_ID('[dbo].[Table2]', 'U') is not null
	drop table [dbo].[Table2]
;
go


CREATE TABLE [dbo].[Table2](
[Id] [int] NOT NULL,
[InstanceID] [int] NOT NULL,
[Content] [nvarchar](50) NULL,
CreationDateTime datetime2 NOT NULL
 CONSTRAINT [PK_Table2] PRIMARY KEY CLUSTERED 
(
	[Id] ASC
)WITH (STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF)
)
GO






if object_ID('[dbo].[OpenChange]', 'P') is not null
drop procedure [dbo].[OpenChange]
go

CREATE PROCEDURE [dbo].[OpenChange]
	@InstanceID INT
AS		
BEGIN
	DECLARE @MaxId INT;
	BEGIN TRY
		BEGIN TRANSACTION
			SELECT @MaxId = (SELECT MAX(Id) FROM Table2 WHERE Content IS NOT NULL)
			IF (@MaxId IS NULL)
				BEGIN
				SELECT @MaxID = 0
				END
			INSERT INTO Table2 (Id, InstanceID,CreationDateTime) VALUES ((@MaxID+1), @InstanceID, SYSDATETIME());
			SELECT MAX(Id) FROM Table2
		COMMIT TRANSACTION
	END TRY
	BEGIN CATCH
			-- See of the open chnage is old, then claim it with an update
			SELECT -1 -- Something else went wrong
		ROLLBACK TRANSACTION
	END CATCH
END
GO





DECLARE @InstanceID INT = 23

-- INSERT INTO Table2 (Id, InstanceID) VALUES (((SELECT MAX(Id) FROM Table2 WHERE Content IS NOT NULL)+1), @InstanceID);
INSERT INTO Table2 (Id, InstanceID,CreationDateTime) VALUES (((SELECT MAX(Id) FROM Table2 WHERE Content IS NOT NULL))+1), @InstanceID, SYSDATETIME());


UPDATE Table2 set Content = 'Updated content' where Id=3;


DECLARE	@return_value int

EXEC	@return_value = [dbo].[OpenChange]
	@InstanceID = 24

GO


if object_ID('[dbo].[CompleteChange]', 'P') is not null
drop procedure [dbo].CompleteChange
go

CREATE PROCEDURE [dbo].CompleteChange
	@Id INT,
	@InstanceID INT,
	@Content nvarchar(50)
AS		
BEGIN
	IF EXISTS(SELECT Id FROM Table2 where Id=@Id AND InstanceID=@InstanceID AND Content IS NULL)
	BEGIN
		UPDATE Table2 set Content = 'Updated content' where Id=@Id AND InstanceID=@InstanceID AND Content IS NULL;
		SELECT 0
		END
		ELSE
		SELECT -1
END
GO

DECLARE	@return_value int

EXEC	@return_value = [dbo].[CompleteChange]
	@Id = 2,
	@InstanceID = 24,
	@Content = 'Moth balls stink'

GO