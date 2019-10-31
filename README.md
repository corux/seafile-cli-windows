# seafile-cli-windows
Seafile CLI application for windows

[![Build status](https://ci.appveyor.com/api/projects/status/atvay3aubsfg30b7/branch/master?svg=true)](https://ci.appveyor.com/project/corux/seafile-cli-windows/branch/master)
[![Build Status](https://travis-ci.com/corux/seafile-cli-windows.svg?branch=master)](https://travis-ci.com/corux/seafile-cli-windows)

This application can be used to upload files to [Seafile](https://www.seafile.com/) using the command line.

## Quick Start

1. Retrieve a personal token for future uploads:
   ```sh
   SeafileCli.exe token --server https://seafile.example.com --username mail@example.com --password your-password
   ```
2. Upload several files to the library "My Library" and directory "Example/Files":
   ```sh
   SeafileCli.exe upload --server https://seafile.example.com --token your-token --library "My Library" --directory "Example/Files" --files /home/user/file1.txt /home/user/file2.txt
   ```
3. Upload a directory, which contains several sub-directories and files, to the library "My Library" and directory "Example/Folder":
   ```sh
   SeafileCli.exe upload --server https://seafile.example.com --token your-token --library "My Library" --directory "Example/Folder" --files /home/user/folder-to-upload/**/* --keep-folders
   ```
