ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [ServerID];
ALTER TABLE [ServerUserJoin] DROP CONSTRAINT [UserID];

DROP TABLE [Server];
DROP TABLE [ServerUserJoin];
DROP TABLE [User];

