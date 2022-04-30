-- phpMyAdmin SQL Dump
-- version 5.0.2
-- https://www.phpmyadmin.net/
--
-- Hôte : 127.0.0.1:3306
-- Généré le : mar. 11 mai 2021 à 23:19
-- Version du serveur :  5.7.31
-- Version de PHP : 7.3.21

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Base de données : `db_rfid`
--

-- --------------------------------------------------------

--
-- Structure de la table `achatparcarte`
--

DROP TABLE IF EXISTS `achatparcarte`;
CREATE TABLE IF NOT EXISTS `achatparcarte` (
  `idAchatParCarte` int(11) NOT NULL AUTO_INCREMENT,
  `idAchats` int(11) NOT NULL,
  `codePermanent` int(11) NOT NULL,
  PRIMARY KEY (`idAchatParCarte`,`idAchats`,`codePermanent`),
  KEY `fk_achatParCarte_achats1_idx` (`idAchats`),
  KEY `fk_achatParCarte_etudiant1_idx` (`codePermanent`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `achatparcode`
--

DROP TABLE IF EXISTS `achatparcode`;
CREATE TABLE IF NOT EXISTS `achatparcode` (
  `idAchatParCode` int(11) NOT NULL AUTO_INCREMENT,
  `code` varchar(6) NOT NULL,
  `etat` varchar(10) NOT NULL,
  `codePermanent` int(11) NOT NULL,
  `idAchats` int(11) NOT NULL,
  PRIMARY KEY (`idAchatParCode`,`codePermanent`,`idAchats`),
  KEY `fk_achatParCode_etudiant1_idx` (`codePermanent`),
  KEY `fk_achatParCode_achats1_idx` (`idAchats`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `achatpersonnel`
--

DROP TABLE IF EXISTS `achatpersonnel`;
CREATE TABLE IF NOT EXISTS `achatpersonnel` (
  `idAchatPersonnel` int(11) NOT NULL,
  `idAchats` int(11) NOT NULL,
  `idPersonnel` int(11) NOT NULL,
  PRIMARY KEY (`idAchatPersonnel`,`idAchats`,`idPersonnel`),
  KEY `fk_achatPersonnel_achats1_idx` (`idAchats`),
  KEY `fk_achatPersonnel_personnel1_idx` (`idPersonnel`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `achats`
--

DROP TABLE IF EXISTS `achats`;
CREATE TABLE IF NOT EXISTS `achats` (
  `idAchats` int(11) NOT NULL AUTO_INCREMENT,
  `montant` int(11) NOT NULL,
  `type` varchar(10) NOT NULL,
  `date` date NOT NULL,
  `heure` varchar(20) NOT NULL,
  `idAgents` int(11) NOT NULL,
  PRIMARY KEY (`idAchats`,`idAgents`),
  KEY `fk_achats_agents1_idx` (`idAgents`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `agents`
--

DROP TABLE IF EXISTS `agents`;
CREATE TABLE IF NOT EXISTS `agents` (
  `idAgents` int(11) NOT NULL AUTO_INCREMENT,
  `nom` varchar(20) NOT NULL,
  `prenom` varchar(45) NOT NULL,
  `position` varchar(100) DEFAULT NULL,
  `profil` varchar(10) NOT NULL,
  PRIMARY KEY (`idAgents`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `anneeacademique`
--

DROP TABLE IF EXISTS `anneeacademique`;
CREATE TABLE IF NOT EXISTS `anneeacademique` (
  `idAnneeAcademique` varchar(10) NOT NULL,
  PRIMARY KEY (`idAnneeAcademique`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `classes`
--

DROP TABLE IF EXISTS `classes`;
CREATE TABLE IF NOT EXISTS `classes` (
  `idClasse` int(11) NOT NULL AUTO_INCREMENT,
  `effectif` int(4) NOT NULL,
  `libelleNiveau` varchar(2) NOT NULL,
  `idFiliere` int(11) NOT NULL,
  `idUfr` int(11) NOT NULL,
  `idAnneeAcademique` varchar(10) NOT NULL,
  PRIMARY KEY (`idClasse`,`libelleNiveau`,`idFiliere`,`idUfr`,`idAnneeAcademique`),
  KEY `fk_classes_niveaux1_idx` (`libelleNiveau`),
  KEY `fk_classes_filieres1_idx` (`idFiliere`,`idUfr`),
  KEY `fk_classes_anneeAcademique1_idx` (`idAnneeAcademique`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `compteagents`
--

DROP TABLE IF EXISTS `compteagents`;
CREATE TABLE IF NOT EXISTS `compteagents` (
  `idCompteAgents` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `password` varchar(254) NOT NULL,
  `etatCompte` tinyint(4) DEFAULT NULL,
  `idAgents` int(11) NOT NULL,
  PRIMARY KEY (`idCompteAgents`,`idAgents`),
  KEY `fk_compteAgents_agents1_idx` (`idAgents`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `compteetudiant`
--

DROP TABLE IF EXISTS `compteetudiant`;
CREATE TABLE IF NOT EXISTS `compteetudiant` (
  `idCompteEtudiant` int(11) NOT NULL AUTO_INCREMENT,
  `email` varchar(45) NOT NULL,
  `password` varchar(254) NOT NULL,
  `etatCompte` tinyint(4) DEFAULT NULL,
  `codePermanent` int(11) NOT NULL,
  PRIMARY KEY (`idCompteEtudiant`,`codePermanent`),
  KEY `fk_compteEtudiant_etudiant1_idx` (`codePermanent`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `datedujour`
--

DROP TABLE IF EXISTS `datedujour`;
CREATE TABLE IF NOT EXISTS `datedujour` (
  `idDateDuJour` date NOT NULL,
  `session` varchar(10) DEFAULT NULL,
  PRIMARY KEY (`idDateDuJour`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `etudiant`
--

DROP TABLE IF EXISTS `etudiant`;
CREATE TABLE IF NOT EXISTS `etudiant` (
  `codePermanent` int(11) NOT NULL AUTO_INCREMENT,
  `nom` varchar(20) NOT NULL,
  `prenom` varchar(50) NOT NULL,
  `dateNaisssance` date NOT NULL,
  `lieuNaissance` varchar(45) NOT NULL,
  `adresse` varchar(254) DEFAULT NULL,
  `idCarte` varchar(15) DEFAULT NULL,
  `etatCarte` tinyint(4) DEFAULT NULL,
  `solde` int(5) DEFAULT NULL,
  `image` longblob,
  PRIMARY KEY (`codePermanent`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `etudiant_par_classe`
--

DROP TABLE IF EXISTS `etudiant_par_classe`;
CREATE TABLE IF NOT EXISTS `etudiant_par_classe` (
  `idClasse` int(11) NOT NULL,
  `codePermanent` int(11) NOT NULL,
  PRIMARY KEY (`idClasse`,`codePermanent`),
  KEY `fk_classes_has_etudiant_etudiant1_idx` (`codePermanent`),
  KEY `fk_classes_has_etudiant_classes1_idx` (`idClasse`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `filieres`
--

DROP TABLE IF EXISTS `filieres`;
CREATE TABLE IF NOT EXISTS `filieres` (
  `idFiliere` int(11) NOT NULL AUTO_INCREMENT,
  `nomfiliere` varchar(254) DEFAULT NULL,
  `ufr_idUfr` int(11) NOT NULL,
  PRIMARY KEY (`idFiliere`,`ufr_idUfr`),
  KEY `fk_filieres_ufr_idx` (`ufr_idUfr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `niveaux`
--

DROP TABLE IF EXISTS `niveaux`;
CREATE TABLE IF NOT EXISTS `niveaux` (
  `libelle` varchar(2) NOT NULL,
  PRIMARY KEY (`libelle`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `passageetudiant`
--

DROP TABLE IF EXISTS `passageetudiant`;
CREATE TABLE IF NOT EXISTS `passageetudiant` (
  `idPassageEtudiant` int(11) NOT NULL AUTO_INCREMENT,
  `heure` varchar(10) NOT NULL,
  `idDateDuJour` date NOT NULL,
  `idplageHoraire` int(11) NOT NULL,
  `codePermanent` int(11) NOT NULL,
  PRIMARY KEY (`idPassageEtudiant`,`idDateDuJour`,`idplageHoraire`,`codePermanent`),
  KEY `fk_passageEtudiant_dateDuJour1_idx` (`idDateDuJour`),
  KEY `fk_passageEtudiant_plageHoraire1_idx` (`idplageHoraire`),
  KEY `fk_passageEtudiant_etudiant1_idx` (`codePermanent`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `passagepersonnel`
--

DROP TABLE IF EXISTS `passagepersonnel`;
CREATE TABLE IF NOT EXISTS `passagepersonnel` (
  `idPassagePersonnel` int(11) NOT NULL,
  `heure` varchar(10) NOT NULL,
  `idPersonnel` int(11) NOT NULL,
  `idplageHoraire` int(11) NOT NULL,
  `idDateDuJour` date NOT NULL,
  PRIMARY KEY (`idPassagePersonnel`,`idPersonnel`,`idplageHoraire`,`idDateDuJour`),
  KEY `fk_passagePersonnel_personnel1_idx` (`idPersonnel`),
  KEY `fk_passagePersonnel_plageHoraire1_idx` (`idplageHoraire`),
  KEY `fk_passagePersonnel_dateDuJour1_idx` (`idDateDuJour`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `personnel`
--

DROP TABLE IF EXISTS `personnel`;
CREATE TABLE IF NOT EXISTS `personnel` (
  `idPersonnel` int(11) NOT NULL AUTO_INCREMENT,
  `nom` varchar(20) NOT NULL,
  `prenom` varchar(45) NOT NULL,
  `profil` varchar(10) DEFAULT NULL,
  `idCarte` varchar(15) DEFAULT NULL,
  `solde` int(5) DEFAULT NULL,
  `etatCarte` tinyint(4) DEFAULT NULL,
  PRIMARY KEY (`idPersonnel`),
  UNIQUE KEY `profil_UNIQUE` (`profil`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `plagehoraire`
--

DROP TABLE IF EXISTS `plagehoraire`;
CREATE TABLE IF NOT EXISTS `plagehoraire` (
  `idplageHoraire` int(11) NOT NULL AUTO_INCREMENT,
  `nomPlage` varchar(20) NOT NULL,
  `debut` varchar(10) NOT NULL,
  `fin` varchar(10) NOT NULL,
  PRIMARY KEY (`idplageHoraire`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `transferts`
--

DROP TABLE IF EXISTS `transferts`;
CREATE TABLE IF NOT EXISTS `transferts` (
  `idTransfert` int(11) NOT NULL AUTO_INCREMENT,
  `codePermanentSource` int(11) NOT NULL,
  `codePermanentDestinataire` int(11) NOT NULL,
  `date` datetime NOT NULL,
  `montant` int(5) NOT NULL,
  PRIMARY KEY (`idTransfert`,`codePermanentSource`,`codePermanentDestinataire`),
  KEY `fk_etudiant_has_etudiant_etudiant2_idx` (`codePermanentDestinataire`),
  KEY `fk_etudiant_has_etudiant_etudiant1_idx` (`codePermanentSource`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

-- --------------------------------------------------------

--
-- Structure de la table `ufr`
--

DROP TABLE IF EXISTS `ufr`;
CREATE TABLE IF NOT EXISTS `ufr` (
  `idUfr` int(11) NOT NULL AUTO_INCREMENT,
  `nomUfr` varchar(100) NOT NULL,
  PRIMARY KEY (`idUfr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8;

--
-- Contraintes pour les tables déchargées
--

--
-- Contraintes pour la table `achatparcarte`
--
ALTER TABLE `achatparcarte`
  ADD CONSTRAINT `fk_achatParCarte_achats1` FOREIGN KEY (`idAchats`) REFERENCES `achats` (`idAchats`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_achatParCarte_etudiant1` FOREIGN KEY (`codePermanent`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `achatparcode`
--
ALTER TABLE `achatparcode`
  ADD CONSTRAINT `fk_achatParCode_achats1` FOREIGN KEY (`idAchats`) REFERENCES `achats` (`idAchats`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_achatParCode_etudiant1` FOREIGN KEY (`codePermanent`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `achatpersonnel`
--
ALTER TABLE `achatpersonnel`
  ADD CONSTRAINT `fk_achatPersonnel_achats1` FOREIGN KEY (`idAchats`) REFERENCES `achats` (`idAchats`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_achatPersonnel_personnel1` FOREIGN KEY (`idPersonnel`) REFERENCES `personnel` (`idPersonnel`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `achats`
--
ALTER TABLE `achats`
  ADD CONSTRAINT `fk_achats_agents1` FOREIGN KEY (`idAgents`) REFERENCES `agents` (`idAgents`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `classes`
--
ALTER TABLE `classes`
  ADD CONSTRAINT `fk_classes_anneeAcademique1` FOREIGN KEY (`idAnneeAcademique`) REFERENCES `anneeacademique` (`idAnneeAcademique`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_classes_filieres1` FOREIGN KEY (`idFiliere`,`idUfr`) REFERENCES `filieres` (`idFiliere`, `ufr_idUfr`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_classes_niveaux1` FOREIGN KEY (`libelleNiveau`) REFERENCES `niveaux` (`libelle`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `compteagents`
--
ALTER TABLE `compteagents`
  ADD CONSTRAINT `fk_compteAgents_agents1` FOREIGN KEY (`idAgents`) REFERENCES `agents` (`idAgents`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `compteetudiant`
--
ALTER TABLE `compteetudiant`
  ADD CONSTRAINT `fk_compteEtudiant_etudiant1` FOREIGN KEY (`codePermanent`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `etudiant_par_classe`
--
ALTER TABLE `etudiant_par_classe`
  ADD CONSTRAINT `fk_classes_has_etudiant_classes1` FOREIGN KEY (`idClasse`) REFERENCES `classes` (`idClasse`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_classes_has_etudiant_etudiant1` FOREIGN KEY (`codePermanent`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `filieres`
--
ALTER TABLE `filieres`
  ADD CONSTRAINT `fk_filieres_ufr` FOREIGN KEY (`ufr_idUfr`) REFERENCES `ufr` (`idUfr`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `passageetudiant`
--
ALTER TABLE `passageetudiant`
  ADD CONSTRAINT `fk_passageEtudiant_dateDuJour1` FOREIGN KEY (`idDateDuJour`) REFERENCES `datedujour` (`idDateDuJour`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_passageEtudiant_etudiant1` FOREIGN KEY (`codePermanent`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_passageEtudiant_plageHoraire1` FOREIGN KEY (`idplageHoraire`) REFERENCES `plagehoraire` (`idplageHoraire`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `passagepersonnel`
--
ALTER TABLE `passagepersonnel`
  ADD CONSTRAINT `fk_passagePersonnel_dateDuJour1` FOREIGN KEY (`idDateDuJour`) REFERENCES `datedujour` (`idDateDuJour`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_passagePersonnel_personnel1` FOREIGN KEY (`idPersonnel`) REFERENCES `personnel` (`idPersonnel`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_passagePersonnel_plageHoraire1` FOREIGN KEY (`idplageHoraire`) REFERENCES `plagehoraire` (`idplageHoraire`) ON DELETE NO ACTION ON UPDATE NO ACTION;

--
-- Contraintes pour la table `transferts`
--
ALTER TABLE `transferts`
  ADD CONSTRAINT `fk_etudiant_has_etudiant_etudiant1` FOREIGN KEY (`codePermanentSource`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION,
  ADD CONSTRAINT `fk_etudiant_has_etudiant_etudiant2` FOREIGN KEY (`codePermanentDestinataire`) REFERENCES `etudiant` (`codePermanent`) ON DELETE NO ACTION ON UPDATE NO ACTION;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
