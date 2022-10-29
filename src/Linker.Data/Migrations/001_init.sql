BEGIN TRANSACTION;
CREATE TABLE IF NOT EXISTS "Links" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Url"	TEXT,
	"Category"	TEXT,
	"Description"	TEXT,
	"Language"	TEXT,
	"LastVisitAt"	TEXT,
	"CreatedAt"	TEXT,
	"ModifiedAt"	TEXT,
	PRIMARY KEY("Id")
);
CREATE TABLE IF NOT EXISTS "Websites" (
	"LinkId"	TEXT NOT NULL UNIQUE,
	"Name"	TEXT,
	"Domain"	TEXT,
	"Aesthetics"	TEXT,
	"IsSubdomain"	INTEGER,
	"IsMultilingual"	INTEGER,
	FOREIGN KEY("LinkId") REFERENCES "Links"("Id")
);
CREATE TABLE IF NOT EXISTS "Articles" (
	"LinkId"	TEXT NOT NULL UNIQUE,
	"Title"	TEXT,
	"Author"	TEXT,
	"Year"	INTEGER,
	"WatchLater"	INTEGER,
	"Domain"	TEXT,
	"Grammar"	TEXT,
	FOREIGN KEY("LinkId") REFERENCES "Links"("Id")
);
CREATE TABLE IF NOT EXISTS "Youtube" (
	"LinkId"	TEXT NOT NULL UNIQUE,
	"Name"	TEXT,
	"Youtuber"	TEXT,
	"Country"	TEXT,
	FOREIGN KEY("LinkId") REFERENCES "Links"("Id")
);
CREATE TABLE IF NOT EXISTS "Tags" (
	"Id"	TEXT NOT NULL UNIQUE,
	"Name"	TEXT NOT NULL UNIQUE,
	"CreatedAt"	TEXT,
	"UpdatedAt"	TEXT,
	PRIMARY KEY("Id")
);
CREATE TABLE IF NOT EXISTS "Links_Tags" (
	"LinkId"	TEXT NOT NULL UNIQUE,
	"TagId"	TEXT NOT NULL UNIQUE,
	FOREIGN KEY("TagId") REFERENCES "Tags"("Id"),
	FOREIGN KEY("LinkId") REFERENCES "Links"("Id"),
	PRIMARY KEY("LinkId","TagId")
);
INSERT INTO "Tags" VALUES ('1','name',NULL,NULL);
COMMIT;
