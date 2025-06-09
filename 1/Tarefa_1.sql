 /*!40100 DEFAULT CHARACTER SET utf8mb4 COLLATE utf8mb4_0900_ai_ci */ /*!80016 DEFAULT ENCRYPTION='N' */
CREATE DATABASE  IF NOT EXISTS `controle_investimentos`;
USE `controle_investimentos`;
-- MySQL dump 10.13  Distrib 8.0.42, for Win64 (x86_64)
--
-- Host: localhost    Database: controle_investimentos
-- ------------------------------------------------------
-- Server version	8.0.42

--
-- Table structure for table `ativo`
--

DROP TABLE IF EXISTS `ativo`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `ativo` (
  `id_ativo` int NOT NULL COMMENT 'Identificador único do ativo.',
  `cod` varchar(15) NOT NULL COMMENT 'Código do ativo, como consta na Bolsa.',
  `nome` varchar(150) NOT NULL COMMENT 'Nome do ativo.',
  PRIMARY KEY (`id_ativo`),
  UNIQUE KEY `id_ativo_UNIQUE` (`id_ativo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `cotacao`
--

DROP TABLE IF EXISTS `cotacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `cotacao` (
  `id_cotac` int NOT NULL,
  `id_ativo` int NOT NULL,
  `prco_uni` decimal(15,2) NOT NULL,
  `dt_hora` datetime(6) NOT NULL,
  PRIMARY KEY (`id_cotac`),
  UNIQUE KEY `id_cotac_UNIQUE` (`id_cotac`),
  KEY `fk_cotacao_ativo1_idx` (`id_ativo`),
  CONSTRAINT `fk_cotacao_ativo1` FOREIGN KEY (`id_ativo`) REFERENCES `ativo` (`id_ativo`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `operacao`
--

DROP TABLE IF EXISTS `operacao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `operacao` (
  `id_opr` int NOT NULL,
  `id_usr` int NOT NULL,
  `id_ativo` int NOT NULL,
  `qtd` decimal(10,2) NOT NULL,
  `prco_uni` decimal(15,2) NOT NULL,
  `tipo_opr` enum('C','V') NOT NULL,
  `corrtg` decimal(14,4) NOT NULL,
  `dt_hora` datetime(6) NOT NULL,
  PRIMARY KEY (`id_opr`),
  UNIQUE KEY `id_opr_UNIQUE` (`id_opr`),
  KEY `fk_operacao_usuario_idx` (`id_usr`),
  KEY `fk_operacao_ativo1_idx` (`id_ativo`),
  CONSTRAINT `fk_operacao_ativo1` FOREIGN KEY (`id_ativo`) REFERENCES `ativo` (`id_ativo`),
  CONSTRAINT `fk_operacao_usuario` FOREIGN KEY (`id_usr`) REFERENCES `usuario` (`id_usr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `posicao`
--

DROP TABLE IF EXISTS `posicao`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `posicao` (
  `id_posic` int NOT NULL,
  `id_usr` int NOT NULL,
  `id_ativo` int NOT NULL,
  `qtd` decimal(10,2) NOT NULL,
  `prco_medio` decimal(15,3) NOT NULL,
  `p_l` decimal(15,3) NOT NULL,
  PRIMARY KEY (`id_posic`),
  UNIQUE KEY `id_posic_UNIQUE` (`id_posic`),
  KEY `fk_posicao_usuario1_idx` (`id_usr`),
  KEY `fk_posicao_ativo1_idx` (`id_ativo`),
  CONSTRAINT `fk_posicao_ativo1` FOREIGN KEY (`id_ativo`) REFERENCES `ativo` (`id_ativo`),
  CONSTRAINT `fk_posicao_usuario1` FOREIGN KEY (`id_usr`) REFERENCES `usuario` (`id_usr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;

--
-- Table structure for table `usuario`
--

DROP TABLE IF EXISTS `usuario`;
/*!40101 SET @saved_cs_client     = @@character_set_client */;
/*!50503 SET character_set_client = utf8mb4 */;
CREATE TABLE `usuario` (
  `id_usr` int NOT NULL,
  `nome` varchar(100) NOT NULL,
  `email` varchar(254) NOT NULL,
  `pct_corrtg` decimal(7,6) NOT NULL,
  PRIMARY KEY (`id_usr`),
  UNIQUE KEY `id_usr_UNIQUE` (`id_usr`)
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_0900_ai_ci;
/*!40101 SET character_set_client = @saved_cs_client */;
