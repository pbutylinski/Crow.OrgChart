# Crow.OrgChart
This is a project that was made in order to help us organize our convention staff in a hierarchical order. As all the existing solutions we could find were meant to be used by corporations and were either bloated, paid or clumsy to use, we decided to develop a simplier version of our own.

Functions (so far):
 - create infinitely nested levels of an organization
 - assign members with their respective roles to each department (along with hierarchy, contact info and notes)
 - visualize the organization's departments hierarchy on a graph
 - search by member or department name
 
The project is open source, so feel free to fork it and adapt to your needs (just please don't forget to credit me as the original author).

## Installation
This project is a self-hosted .NET Core 3.0 application. It should run on any server that supports it, and all the required runtime libraraies should be contained in the output after build.

By default `AspNetCoreHostingModel` is set to `OutOfProcess`, but you can change it in the project file.

## Data storage
Right now app stores its data in `data.json` file locaten in the main directory. Therefore, it must be able to create the file during the first launch.

If you wish to use other means of storage (e.g. database), you need to implement `IOrganizationStorageRepository` interface and swap the DI implementation in `Startup.cs`.

## Securing the app
The app is not being password protected by default. You can do it by either using `.htaccess` file, or use a built-in equivalent.

In `appsettings.json` you can see there are two fields named `SecurityUsername` and `SecurityPassword` (they are empty by default, so the authentication is disabled).

```
{
  ...
  "SecurityUsername": "your-username",
  "SecurityPassword": "your-password"
}
```
After filling the properties with username/password, the app will now prompt you for them on each new session. **As they are mimicking `.htaccsess` file, the default will only take into account the first 6 symbols.**
 
## Author
Paweł "Lemurr" Butyliński, [Lemur Solutions (formerly lemurr.pl)](https://lemursolutions.pl)

## 3rd party libraries
 - [Newtonsoft.Json](https://www.newtonsoft.com/json)
 - [JQuery OrgChart](https://www.jqueryscript.net/demo/Create-An-Editable-Organization-Chart-with-jQuery-orgChart-Plugin/)
