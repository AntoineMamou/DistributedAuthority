# Projet Multijoueur Collaboratif

**MAMOU Antoine**

## 0. Lien du GitHub

https://github.com/AntoineMamou/DistributedAuthority

## 1. Fonctionnalités

Ce projet propose un environnement partagé en temps réel entre deux joueurs PC (clavier/souris) ou entre un joueur PC et un joueur en Réalité Virtuelle.

- **Instanciation dynamique** : Les joueurs PC peuvent faire apparaître de nouveaux objets interactifs (cube) partagés sur le réseau.
- **Gestion de l'autorité** : Transfert dynamique de la propriété des objets, permettant une manipulation sans conflit physique. Le serveur récupère l'autorité lorsqu'un joueur quitte la session.
- **Synchronisation réseau** : États des objets (cyan au survol, jaune à la saisie) via des appels RPC, informant tous les utilisateurs des interactions en cours.

## 2. Lancement et utilisation avec deux joueurs PC local

1. Lancez le jeu sur une première instance
2. Renseignez votre profil et le nom de la session, puis cliquez sur "Create or Join Session"
3. Vous apparaissez dans le monde
4. Lancez une nouvelle instance du jeu
5. Renseignez un autre profil et le même nom de session
6. Vous apparaissez dans le monde

**Contrôles** : 
- Maintenez "Alt" + ZQSD (ou les flèches) pour se déplacer dans le monde
- Maintenez "Alt" + molette pour déplacer le curseur 3D
- Appuyez sur "P" pour créer un objet
- Cliquez-glissez pour manipuler un cube

## 3. Lancement et utilisation avec un joueur hôte PC et un joueur VR

> **Note** : La connexion entre le casque et le PC n'est pas fonctionnelle, mais tous les éléments pour le crossPlatform sont implémentés.

### Configuration

1. Connectez le casque et le PC au même réseau
2. Renseignez l'adresse IP du PC dans Unity : `NetworkManager > Distributed Authority Transport > Address` (scène PC et VR)
3. Définissez l'IP du PC dans `NetworkManager > VRConnectionManager`
4. Créez un build pour PC et un build pour le casque

### Côté PC (Hôte)

1. Lancez une instance du jeu
2. Renseignez votre profil et le nom de session (`sessionPCVR`)
3. Cliquez sur "Create or Join Session"

### Côté VR (Client)

1. Une fois la session lancée sur PC, lancez l'application sur le casque
2. Pointez le cube interactif avec la manette et cliquez pour rejoindre

**Contrôles VR** : Approchez votre main d'un objet (cyan) et saisissez-le (jaune) pour interagir de manière synchronisée avec le joueur PC.
