# 🎮 Lancement du projet avec le Meta Quest 3

Ce guide explique deux méthodes pour lancer le projet : **avec** ou **sans** l'utilisation de **Meta Quest Link**.

## 🔗 Lancement du projet AVEC Meta Quest Link

### ✅ Prérequis
- Casque **Meta Quest 3**
- **Câble Link**
- Application **Meta Quest Link** (avec un compte Meta connecté)
- **Unity** et **Unity Hub**

### 🚀 Étapes
1. Ouvrir le projet dans **Unity**
2. Lancer l'application **Meta Quest Link** sur le PC
3. Connecter le casque Meta Quest 3 au PC via le **câble Link**
4. Vérifier la connexion dans **Meta Quest Link > Appareils**  
   > Si le casque n’est pas détecté :  
   > - Aller dans **Paramètres > Général > Exécution OpenXR**  
   > - Cliquer sur **Définir Meta Quest Link comme runtime par défaut**
5. Mettre le casque, choisir **Link** dans le menu, puis attendre l'arrivée dans l'interface Link
6. Dans l'éditeur Unity, cliquer sur **Play**  
   > L’application se lancera automatiquement dans le casque
7. Cliquer sur **Stop** pour arrêter l'expérience

### ❗ En cas de crash
1. Fermer Unity et rouvrir le projet via **Unity Hub**
2. Reconnecter le câble Link et vérifier la connexion dans l’application **Meta Quest Link** et dans le casque
3. Si le problème persiste, **redémarrer le casque**

## 📦 Lancement du projet SANS Meta Quest Link

### ✅ Prérequis
- Casque **Meta Quest 3**
- **Unity** et **Unity Hub**

### 🚀 Étapes
1. Ouvrir le projet dans **Unity**
2. Aller dans **File > Build Settings**
3. Vérifier que la plateforme sélectionnée est bien **Android**
4. Dans la liste des scènes, **cocher uniquement la scène _Museum_**
5. Cliquer sur **Build and Run**
6. Attendre que l’application soit transférée sur le **Meta Quest 3**
7. Lancer manuellement l’application depuis le casque

### ❗ En cas de crash
1. Fermer Unity et rouvrir le projet via **Unity Hub**
2. Vérifier la connexion du casque et s'assurer que le câble fonctionne correctement
3. Si le problème persiste, **redémarrer le casque**

# Assets

1. Pour retrouver les prefabs (3D) : **Assets > _Prefabs**
2. Pour retrouver les sons : **Assets > _Sounds**
