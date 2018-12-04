---
title: Testrapport
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
date: November 2014
...

# Inleiding

Dit document bevat het testrapport van ons project. In dit testrapport worden alle tests nog een keer beschreven en wat wij verwachten als uitkomst. Daarnaast komen de werkelijke testresultaten en ten slot nog een kleine conclusie.

# 1. De tests

## TestID 1: Zichtbare avatar

Na het openen van de applicatie, verschijnt er een scherm met een avatar

|-|-|
| Verwachte uitkomst | Pass |
| Daadwerklijke uitkomst | Pass |


Na het openen van een module, verschijnt de avatar verkleind in de linkeronderhoek

|-|-|
| Verwachte uitkomst | Pass |
| Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Wouter en Robert op 26-11-2014 en ze zijn geslaagd.

##TestID 2: Pratende avatar

Na het openen van de applicatie, spreekt de avatar een welkomstteskt uit

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Bij een tekst van 100 of meer karakters (zonder leestekens)geeft het programma aan dat de teskt te lang is

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Bij een tekst van 100 of meer karakters (met leestekens) splitst de applicatie de zin in stukjes en spreekt ze ze achter elkaaruit

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Zonder internetverbinding spreekt de avatar de zinnen gewoon uit, mits ze al een keer eerder zijn uitgesproken mét internetverbinding

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Als er op de avatar geklikt wordt twerijl ze praat, stopt ze met praten

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en Robert op 27-11-2014 en ze zijn geslaagd.

##TestID 3: Weergave dialoog

Na het openen van de applicatie of een module, verschijnt de bij de applicatie of module horende tekst op het scherm

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Het XML-tesktbestand bevat alle tekst met de juiste categorieën en teksten

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Heropenen van de applicatie of een module zorgt voor variatie in de weergegeven teksten

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

Aanpassingen aan de tekst in het XML-tekstbestand worden correct weergegeven

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Evan en Joram op 26-11-2014 en ze zijn geslaagd.

##TestID 4: Geanimeerde avatar

Wanneer de applicatie is geopend, knippert de avatar nu en dan met haar ogen

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer de avater praat, beweegt haar mond

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Roelof en Wouter op 4-12-2014 en ze zijn geslaagd.

##TestID 5: Overzicht locaties

Wanneer de kaartmodule is geopend, verschijnt er een scherm met de plattegrond van het T-gebouw

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Bij het klikken op een verdieping, wordt de plattegrond van de geokzen verdieping getoond

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer er weer een verdieping geselecteerd wordt, verschijnen er niet meerdere plattegronden

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Nadat de kaartmodule is herstart, onthoudt hij de laatste activiteit en gaat hij verder waar hij was gebleven

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Na het drukken op de home-button, verwijnt de module van het scherm

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en JanJaap op 17-12-2014 en ze zijn geslaagd.

##TestID 6: Selecteren lokaal

Na het invullen van een lokaal op de numpad in de kaartmodule, verschijnt er een knipperende rode cirkel om het ingevulde lokaal

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Als er op de plattegrond geklikt wordt, verschijnt er een knipperende rode cirkel om het lokaal wat het dichtst bij de klik staat

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Zodra er een verdieping ingevuld wordt op de numpad binnen de kaartmodule, opent de plattegrond voor de verdieping

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en Evan op 17-12-2014 en ze zijn geslaagd.

##TestID 7: Weergave route lokaal

Wanneer een lokaal is ingevuld op de numpad binnen de kaartmodule, spreekt de avatar een beschrijving uit om naar het ingevulde lokaal te komen

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer een lokaal is ingevuld op de numpad binnen de kaartmodule, toont de applicatie die uitgesproken tekst ook op het scherm

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conslusie**

Deze tests zijn uitgevoerd door Robert en JanJaap op 17-12-2014 en ze zijn geslaagd.

##TestID 8: Idle

Nadat de applicatie is geopend en er een tijdje is gewacht, gaat het scherm naar een random module die 'rotatable' is

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Nadat de applicatie is geopend en er een tijdje is gewacht, gaat het scherm naar een module die niet 'rotatable' is

|-|-|
|  Verwachte uitkomst | Fail |  
|  Daadwerklijke uitkomst  | Fail |

Nadat de aplicatie is geopend en er een tijdje is gewacht, opent hetzelfde scherm weer opnieuw

|-|-|
|  Verwachte uitkomst | Fail |  
|  Daadwerklijke uitkomst  | Fail |

De avatar praat maar één keer in de tien minuten als ze van module switched

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Joram en Roelof op 8-1-2015 en ze zijn geslaagd.

##TestID 9: Twitterfeed

Wanneer de Twittermodule wordt geopend, verschijnt er een scherm met de vier recentste tweets onder elkaar zijn weergegeven

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Als er op een accountnaam aan de rechterkant wordt geklikt, verschijnen er vier tweets van de geselecteerde accountnaam

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Als er op een hashtag aan de rechterkant wordt geklikt, verschijnen er vier tweets van de geselecteerde hashtag

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Als er vijf minuten gewacht wordt nadat de Twittermodule is geopend, krijgen de tweets een update

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Zodra er op ctrl+N wordt gedrukt, verschijnt er een schermpje waar een wachtwoord in moet worden gevoerd

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer het juiste wachtwoord is ingevuld in het schermpje, verschijnt er een paneeltje waarin de accountnamen en hashtags beheert kunnen worden

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Bij verandering van accountnaam of hashtag update de module zijn tweets

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer de Twittermodule wordt geopend zonder internetverbinding, opent de module zonder tweets

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Fail |

Als er op de home-button geklikt wordt, verdwijnt de module van het scherm

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conslusie**

Deze tests zijn uitgevoerd door Arjan en JanJaap op 8-1-2015 en ze zijn niet allemaal geslaagd op de eerste poging. De bug is gefixt op 8-1-2015. 

##TestID 10: Weeroverzicht

Bij opening van de applicatie, verschijnt er in de rechterbovenhoek een iccontje met de huidige weersvoorspelling van Zwolle

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Bij opening van de applicatie, verschijnt er in de rechterbovenhoek een icoontje met de huidige temperatuur van Zwolle

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

De icoontjes met de huidige weersvoorspelling en de huidige temperatuur zijn in elke module zichtbaar

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Bij opening van de weermodule, verschijnt een scherm met de weersvoorpelling van de huidige dag en de drie dagen daarna

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer de weermodule wordt geopend zonder internetverbinding, opent de module zonder weersvoorspelling

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Fail

Na 5 minuten update het weer

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass | 

Als er op de home-button geklikt wordt, verdwijnt de module van het scherm

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Wouter en Evan op 8-1-2015 en ze zijn niet allemaal geslaagd op de eerste poging. De bug is gefixt op 8-1-2015.

##TestID 11: Applicatiebeveiliging

Als de applicatie eenmaal draait, is er geen mogelijkheid om, als niet-beheerder, de applicatie te sluiten

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**
Deze tests zijn uitgevoerd door Wouter en Roelof op 8-1-2015 en ze zijn geslaagd.

##TestID 12: Vertrektijden trein

Bij openen van de vertrektijdenmodule, verschijnt er een scherm met daarop de actuele vertrektijden van de treinen op het station van Zwolle

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Als de vertrektijdmodule geopend wordt zonder internetverbinding, opent de module zonder treintijden

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Fail |

Zodra een trein vertrokken is, verdwijnt die trein van het overzicht

|-|-|
|  Verwachte uitkomst | Pass |  
|  Daadwerklijke uitkomst  | Pass |

Wanneer er op de home-button geklikt wordt, verdwijnt de module van het scherm

|-|-|
|  Verwachte uitkomst | Pass |
|  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en JanJaap op 9-1-2015 en ze zijn niet allemaal geslaagd op de eerste poging. De bug is gefixt op 9-1-2015.

# Conclusie

Bijna al onze tests zijn in één keer geslaagd, met uitzondering van een paar testjes. Elke test is uitgevoerd door in ieder geval twee personen, zodat meer mensen naar de dingen hebben gekeken die anderen hebben geprogrammeerd. Niemand heeft daarom ook iets getest wat hij zelf heeft gerealiseerd, want het geeft geen goed inzicht als iedereen zijn eigen dingen ook test. Tevens kijk je dan niet naar andersmans code, maar alleen naar die van jezelf. 
