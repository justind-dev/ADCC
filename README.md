# ADDC
 Active Directory Command Center - Simplifying every day AD tasks.

## Features

- Mutli Domain support configured via the App.Config file. 
- User Unlocks
- Find user distinguished name


## How to use
- Edit the App.Config file string "domain1.com,domain2.com" to reflect your domains you manage.
- If you do not use more than one domain remove all entries and leave it as "null"
- In windows credential manager add a *generic credential* for your domain in the format:
             Name: domain1.com
             Username: username@domain1.com
             Password: your password for that domain.
- You will need one for each domain you put in the previous list. You dont need to do one for your default domain that the computer is connected to unless you are logged in with a user without domain priveleges. 
- From there you will open the application and select which domain you'd like to perform a function against from the upper right hand drop down. 
- It defaults to the locally connected domain.
- Thats it!
- *bugs may exist, this project is a work in progress!!*

## Up Coming Features

- User Password Reset
- Computer Reports (Configurable filters such as by OS, Disabled computers, etc...)
- User Reports (Configurable filters such as Inactive for x days, disabled users, users with passwords which dont expire, etc..)
- Batch user creation
- Batch computer creation
- and many more hopefully. I am open to suggestions so if you would like to contribute feel free to submit a PR or open an issue for the feature you would find useful.

