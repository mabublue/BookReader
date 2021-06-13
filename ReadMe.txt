BookReader
==========



EF Migrations
=============

Initial Create:
1. Right click on your main project and choose "Set as startup project"
2. Run tools>nuget package manager>package manager console
3. Select the project 'Data' which contains the DbContext from drop-down list
4. Run 'CD Data'
5. Run 'Add-Migration InitialCreate'


Subsequent Migrations:
1. Right click on your main project and choose "Set as startup project"
2. Run tools>nuget package manager>package manager console
3. Select the project 'Data' which contains the DbContext from drop-down list
4. Run: 'Add-Migration <Migration Name>' (replace the name of the migration you want to create)

Alternative to Add-Migration from CLI:
- dotnet ef migrations add MigrationName -s Book-Reader -p Book-Reader.Data

