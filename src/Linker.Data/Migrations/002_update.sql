BEGIN TRANSACTION;
DROP TABLE IF EXISTS "Tags";
CREATE TABLE IF NOT EXISTS "Tags" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	"CreatedAt"	TEXT,
	"ModifiedAt"	TEXT,
	PRIMARY KEY("Id")
);
DROP TABLE IF EXISTS "Users";
CREATE TABLE IF NOT EXISTS "Users" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Username"	TEXT NOT NULL UNIQUE,
	"Password"	TEXT NOT NULL,
	"Role"	TEXT NOT NULL,
	"Status"	TEXT NOT NULL,
	"CreatedAt"	TEXT NOT NULL,
	"ModifiedAt"	TEXT NOT NULL,
	"DateOfBirth"	DATETIME DEFAULT 1970-01-01,
	PRIMARY KEY("Id")
);
DROP TABLE IF EXISTS "Links_Tags";
CREATE TABLE IF NOT EXISTS "Links_Tags" (
	"LinkId"	TEXT NOT NULL,
	"TagId"	TEXT NOT NULL,
	PRIMARY KEY("LinkId","TagId"),
	FOREIGN KEY("TagId") REFERENCES "Tags"("Id")
);
DROP TABLE IF EXISTS "Articles";
CREATE TABLE IF NOT EXISTS "Articles" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Title"	TEXT,
	"Author"	TEXT,
	"Year"	INTEGER,
	"WatchLater"	INTEGER,
	"Domain"	TEXT,
	"Grammar"	TEXT,
	"Url"	TEXT,
	"Category"	TEXT,
	"Description"	TEXT,
	"Language"	TEXT,
	"Rating"	TEXT,
	"LastVisitAt"	TEXT,
	"CreatedAt"	TEXT,
	"ModifiedAt"	TEXT,
	"CreatedBy"	TEXT,
	PRIMARY KEY("Id")
);
DROP TABLE IF EXISTS "Websites";
CREATE TABLE IF NOT EXISTS "Websites" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Name"	TEXT,
	"Domain"	TEXT,
	"Aesthetics"	TEXT,
	"IsSubdomain"	INTEGER,
	"IsMultilingual"	INTEGER,
	"Url"	TEXT,
	"Category"	TEXT,
	"Description"	TEXT,
	"Language"	TEXT,
	"Rating"	TEXT,
	"LastVisitAt"	TEXT,
	"CreatedAt"	TEXT,
	"ModifiedAt"	TEXT,
	"CreatedBy"	TEXT,
	PRIMARY KEY("Id")
);
DROP TABLE IF EXISTS "Youtube";
CREATE TABLE IF NOT EXISTS "Youtube" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Name"	TEXT,
	"Youtuber"	TEXT,
	"Country"	TEXT,
	"Url"	TEXT,
	"Category"	TEXT,
	"Description"	TEXT,
	"Language"	TEXT,
	"Rating"	TEXT,
	"LastVisitAt"	TEXT,
	"CreatedAt"	TEXT,
	"ModifiedAt"	TEXT,
	"CreatedBy"	TEXT,
	PRIMARY KEY("Id")
);
COMMIT;
