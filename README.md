# ShrinkLink
Проект представляет собой MVC приложение.
Главное приложение ShrinklinkApp
Для запуска требуется appsettings.json вида:


{
  "ConnectionStrings": {
    "Default": ""
  },
  "UserSecrets": {
    "PasswordSalt": ""
  },
  "ShrinkLink": {
    "Length": "9",
    "ExpiryTimeMin": "2880",
    "CharSet": "QWERTYUIOPASDFGHJKLZXCVBNMqwertyuiopasdfghjklzxcvbnm01234567890_"
  },
  "PageSize": {
    "Default": "4"
  },
  "Serilog": {
    "LogFilePath": ""
  },

  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}

База данных в проекте ShrinklinkDb. 
Необходимо создать БД MS SQL (add-migration XXX, update-database)  и создать вручную роли Customer, Admin.


Функционал:
Пользователь (может быть как анонимный, так и авторизованный) вводит оригинальную ссылку,
которую желает сократить. Система выдает сокращенную ссылку.
Если ссылка есть в базе, новая не генерируется. При попытке генерации обновляется ее срок экспирации.
Присутствует авторизация, кабинет пользователя с редактированием настроек и времени экспирации ссылок.
Так как одна и таже ссылка может быть добавлена несколькими пользователями, ее полное удаление происходит только после ее удаления всеми пользователями.
Для удаления "просроченных" ссылок предполагается использовать HangFire.

