---
title: 'Install ShadowCat'
date: 2019-02-11T19:27:37+10:00
draft: false
weight: 3
---

## Get your first ShadowCat

We are currently offer ShadowCat as a free VPN service. You can get your first ShadowCat by submitting a request to us with your email address via this form.

After submitting your interest you will receive a ShadowCat VPN seed which can be added to your OpenVPN Client

## Add ShadowCat to OpenVPN

Please follow the instruction below based on your device and operating system.

### Windows

Import file via your Window OpenVPN Client.

### Mac

Import file via your Mac OpenVPN Client.

### IOS

Import file via your IOS OpenVPN App.

### Android

Import file via your Android OpenVPN App.


You can download the .zip file located here https://github.com/JugglerX/hugo-whisper-theme/archive/master.zip.

Extract the downloaded .zip inside the `themes` folder. Rename the extracted folder from `hugo-whisper-theme-master` -> `hugo-whisper-theme`. You should end up with the following folder structure `mynewsite/themes/hugo-whisper-theme`

## Add example content

The fastest way to get started is to copy the example content and modify the included `config.toml`

### Copy exampleSite contents

Copy the entire contents of the `exampleSite` folder to the root folder of your Hugo site _(the folder with the README.md)_.

### Update config.toml

After you copy the `config.toml` into the root folder of your Hugo site you will need to update the `baseURL`, `themesDir` and `theme` values in the `config.toml`

```
baseURL = "/"
themesDir = "themes"
theme = "hugo-whisper-theme"
```

## Run Hugo

After installing the theme for the first time, generate the Hugo site.

```
hugo
```

For local development run Hugo's built-in local server.

```
hugo server
```

Now enter [`localhost:1313`](http://localhost:1313) in the address bar of your browser.
