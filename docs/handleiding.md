---
title: Ontwikkelaarshandleiding
group: Team Awesomer - ICTSE1a - KBS1
author:
- name: Arjan van der Weide
  snr: S1070619
- name: Robert Leeuwis
  snr: S1062367
- name: Joram Schrijver
  snr: S1067040
- name: Wouter Vogelzang
  snr: S1073684
- name: Evan van Urk
  snr: S1071256
- name: Janjaap Ree
  snr: S1066187
- name: Roelof Roos
  snr: S1073508
version: 0.1
date: Januari 2015
---

# Het proces

Bij het opstarten van de applicatie worden alle `*.dll` bestanden in de map
`modules` geladen. Uit deze bestanden worden modules geladen die op het
hoofdscherm weergegeven zullen worden.

Modules die ingeladen zullen worden zijn klassen die de interface `IModule`
implementeren. Om deze weer te geven worden klassen ingeladen die de interface
`IView` implementeren. Als laatste is er nog de mogelijkheid om een kleine
widget linksbovenin het scherm weer te geven, hiervoor moet de interface
`IBackgroundWidget` geïmplementeerd worden.

# De onderdelen {#parts}

## IModule

Een module moet een aantal eigenschappen en methoden implementeren:

- `string Name`: De naam van de module waarmee andere modules ernaar kunnen
  refereren. Deze naam kan het best uit een Engels woord of meerdere Engelse
  woorden bestaan.
- `Bitmap Icon`: Een icoontje dat weergegeven zal worden op het hoofdscherm. Kan
  ook `null` zijn, dan zal een standaard icoontje worden weergegeven.
- `string DisplayName`: De naam van de module die op het hoofdscherm weergegeven
  zal worden. Deze naam kan het best bestaan uit één of meerdere Nederlandse
  woorden.
- `bool Rotatable`: Of deze module mee moet worden genomen in de automatische
  rotatie van modules als het scherm niet in gebruik is.
- `void Initialize(IController controller)`: Een methode die, bij de start van
  het programma, wordt aangeroepen om eventuele initializatie van de module uit
  te voeren.
- `void Start()`: Een methode die wordt aangeroepen zodra de module de actieve
  module wordt.
- `void Stop()`: Een methode die wordt aangeroepen zodra de module niet langer
  de actieve module is.

## IView

Een view moet ook een aantal eigenschappen en methoden implementeren:

- `Type ModuleType`: Dit is het type van de module die deze view kan weergeven.
  Als er bijvoorbeeld sprake is van een `WeatherView` die de weergave van een
  `WeatherModule` verzorgt, dan zal dit `typeof(WeatherModule)` moeten zijn.
- `IBackgroundView BackgroundView`: Een instantie van `IBackgroundView` die de
  achtergrond voor deze view verzorgt. Voor de meeste modules is dit een
  `SubBackground`, maar voor het hoofdscherm is het een `MainBackground`.
- `void Initialize(IController controller)`: Een methode die, bij de start van
  het programma, wordt aangeroepen om eventuele initializatie voor deze view uit
  te voeren.
- `void Attach(IModule module)`: Een methode die wordt aangeroepen zodra de
  module waar deze view bij hoort de actieve module wordt. De parameter `module`
  is van het type aangegeven door de eigenschap `ModuleType`.
- `void Detach()`: Een methode die wordt aangeroepen zodra de module waar deze
  view bijhoort niet meer actief is en deze view dus niet meer weergegeven
  wordt.
- `void Render(Grahpics graphics, Rectangle rectangle)`: Een methode die een
  groot aantal keer per seconde aangeroepen zal worden om de inhoud van de
  module op het scherm te tekenen. De parameter `graphics` moet gebruikt worden
  voor het tekenen en de parameter `rectangle` representeert het gebied waarin
  getekend moet worden.

## IController

Verscheidene methoden krijgen als parameter een `IController` mee. Dit is een
object dat het draaien van het programma regelt. Hij exporteert een aantal
events waar modules of views methoden aan kunnen hangen en een paar centrale
objecten in het programma. Hij bevat de volgende eigenschappen:

- `ModuleManager ModuleManager`: Een instantie van ModuleManager die gebruikt
  kan worden om informatie over andere modules te vinden.
- `void SelectModule(IModule module)`: Laat de controller een andere module de
  actieve module maken. Een module-object om hieraan mee te geven kan verkregen
  worden door de methode `GetModule(string name)` van `ModuleManager`.
- `Avatar Avatar`: Een instantie van `Avatar` die in het programma weergegeven
  wordt en die teksten uitspreekt.

Ook bevat `IController` een aantal events:

- `event TickHandler Tick`: Een event dat tot 100 keer per seconde aangeroepen
  wordt om de staat van het programma bij te werken. Dit event wordt gereset
  wanneer de actieve module wisselt.
- `event BackgroundUpdateHandler BackgroundUpdate`: Een event dat één keer per 5
  minuten wordt aangeroepen op een andere thread om gegevens op de achtergrond
  bij te werken. Deze kan bijvoorbeeld gebruikt worden voor het updaten van
  informatie waarvoor informatie van het internet gehaald moet worden.
- `event TouchHandler Touch`: Een event dat wordt aangeroepen wanneer een
  gebruiker op het scherm tikt. Dit event wordt gereset wanneer de actieve
  module wisselt.
- `event TouchHandler DragStart`: Een event dat wordt aangeroepen wanneer een
  gebruiker begint met het aanraken van het scherm.
- `event TouchHandler DragEnd`: Een event dat wordt aangeroepen wanneer een
  gebruiker stopt met slepen over het scherm.
- `event DragHandler Drag`: Een event dat wordt aangeroepen wanneer een
  gebruiker sleept over het scherm.

## IBackgroundWidget

Om linksboven in de hoek een kleine widget weer te geven moet de interface
`IBackgroundWidget` geïmplementeerd worden. Deze bevat de volgende eigenschappen
en methoden:

- `string ModuleName`: De naam van de module waar deze widget bijhoort.
- `void Render(Graphics graphics, Rectangle rectangle)`: Een methode die wordt
  aangeroepen om de widget te tekenen. De parameter `graphics` moet gebruikt
  worden om te tekenen en de parameter `rectangle` geeft het gebied aan waarin
  getekend moet worden.
- `void Initialize(IController controller, IModule module)`: Een methode die bij
  de start van het programma wordt aangeroepen om initialisatie uit te voeren.
  De parameter `module` is de module met de naam die aangegeven wordt door
  `ModuleName`.

Een `IBackgroundWidget` heeft geen `Attach()` of `Detach()` methoden omdat hij
altijd in beeld is.

# Aan de slag

Een module bouwen is eigenlijk heel eenvoudig:

1. Maak een nieuw project aan.
2. Voeg `Project Novi` als reference toe. (Zet eventueel de eigenschap
   `Copy Local` op false.)
3. Stel, in de "Properties" van het project, onder "Build" het "Output path" zo
   in dat de output van de compiler wordt geplaatst in een submap van de map
   `modules`.
4. Voeg twee klassen toe, een `...Module` en een `...View`. Laat de module
   de interface `IModule` implementeren en de view de interface `IView`
   implementeren.
5. Implementeer de eigenschappen en methoden zoals
   [hierboven beschreven](#parts).
