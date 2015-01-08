---
title: Testrapport
group: Team Awesomer - ICTSE1a - KBS1
authors:
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


    | Na het openen van de applicatie, verschijnt er een scherm met een avatar |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 


    | Na het openen van een module, verschijnt de avatar verkleind in de linkeronderhoek |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

**Conclusie**

Deze tests zijn uitgevoerd door Wouter en Robert op 26-11-2014 en ze zijn geslaagd.

##TestID 2: Pratende avatar

    | Na het openen van de applicatie, spreekt de avatar een welkomstteskt uit |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

    | Bij een tekst van 100 of meer karakters (zonder leestekens)geeft het programma aan dat de teskt te lang is |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

    | Bij een tekst van 100 of meer karakters (met leestekens) splitst de applicatie de zin in stukjes en spreekt ze ze achter elkaaruit |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

    | Zonder internetverbinding spreekt de avatar de zinnen gewoon uit, mits ze al een keer eerder zijn uitgesproken mét internetverbinding |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

    | Als er op de avatar geklikt wordt twerijl ze praat, stopt ze met praten |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en Robert op 27-11-2014 en ze zijn geslaagd.

##TestID 3: Weergave dialoog

    | Na het openen van de applicatie of een module, verschijnt de bij de applicatie of module horende tekst op het scherm |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

    | Het XML-tesktbestand bevat alle tekst met de juiste categorieën en teksten |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass | 

    | Heropenen van de applicatie of een module zorgt voor variatie in de weergegeven teksten |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Aanpassingen aan de tekst in het XML-tekstbestand worden correct weergegeven |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Evan en Joram op 26-11-2014 en ze zijn geslaagd.

##TestID 4: Geanimeerde avatar

    | Wanneer de applicatie is geopend, knippert de avatar nu en dan met haar ogen |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Wanneer de avater praat, beweegt haar mond |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en JanJaap op 4/12/2014 en ze zijn geslaagd.

##TestID 5: Overzicht locaties

    | Wanneer de kaartmodule is geopend, verschijnt er een scherm met de plattegrond van het T-gebouw |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Bij het klikken op een verdieping, wordt de plattegrond van de geokzen verdieping getoond |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Wanneer er weer een verdieping geselecteerd wordt, verschijnen er niet meerdere plattegronden |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Nadat de kaartmodule is herstart, onthoudt hij de laatste activiteit en gaat hij verder waar hij was gebleven |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Na het drukken op de home-button, verwijnt de module van het scherm |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Arjan en Evan op 17-12-2014 en ze zijn geslaagd.

##TestID 6: Selecteren lokaal

    | Na het invullen van een lokaal op de numpad in de kaartmodule, verschijnt er een knipperende rode cirkel om het ingevulde lokaal |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Als er op de plattegrond geklikt wordt, verschijnt er een knipperende rode cirkel om het lokaal wat het dichtst bij de klik staat |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

    | Zodra er een verdieping ingevuld wordt op de numpad binnen de kaartmodule, opent de plattegrond voor de verdieping |
    |---|---|
    |  Verwachte uitkomst | Pass |  
    |  Daadwerklijke uitkomst  | Pass |

**Conclusie**

Deze tests zijn uitgevoerd door Robert en JanJaap op 17/12/2014 en ze zijn geslaagd.
