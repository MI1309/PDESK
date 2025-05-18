-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 18, 2025 at 02:24 AM
-- Server version: 10.4.32-MariaDB
-- PHP Version: 8.2.12

SET SQL_MODE = "NO_AUTO_VALUE_ON_ZERO";
START TRANSACTION;
SET time_zone = "+00:00";


/*!40101 SET @OLD_CHARACTER_SET_CLIENT=@@CHARACTER_SET_CLIENT */;
/*!40101 SET @OLD_CHARACTER_SET_RESULTS=@@CHARACTER_SET_RESULTS */;
/*!40101 SET @OLD_COLLATION_CONNECTION=@@COLLATION_CONNECTION */;
/*!40101 SET NAMES utf8mb4 */;

--
-- Database: `pulsa`
--

-- --------------------------------------------------------

--
-- Table structure for table `account`
--

CREATE TABLE `account` (
  `id` int(11) NOT NULL,
  `username` varchar(30) NOT NULL,
  `password` varchar(30) NOT NULL,
  `role` varchar(30) NOT NULL,
  `alamat` varchar(255) DEFAULT NULL,
  `no_telpon` varchar(255) DEFAULT NULL,
  `email` varchar(255) DEFAULT NULL,
  `nama_kasir` varchar(255) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data for table `account`
--

INSERT INTO `account` (`id`, `username`, `password`, `role`, `alamat`, `no_telpon`, `email`, `nama_kasir`) VALUES
(1, 'admin', 'admin', 'admin', 'idk man', '2147483647', 'admin@kasir.com', 'bro admin'),
(23, 'imron', 'imron', 'kasir', 'jl kedinding lor gang sedap malam no 19, surabaya', '089514366445', 'imron@gmail.co', 'Muhammad Imron'),
(25, 'rido', 'RidhoHusein678', 'kasir', 'KEC Candi sydney', '08221234568789', 'imron@imron.com', 'Ridho Husein Carpenther'),
(26, 'vito123', 'vito', 'kasir', 'jl candi sukodono blblabala', '0809123892381', 'vito@gmail.co', 'vito basuki');

-- --------------------------------------------------------

--
-- Table structure for table `admin_product`
--

CREATE TABLE `admin_product` (
  `id` int(11) NOT NULL,
  `nama_produk` varchar(100) DEFAULT NULL,
  `harga_jual` int(250) DEFAULT NULL,
  `harga_kulak` int(250) NOT NULL,
  `tanggal_restock` date NOT NULL DEFAULT current_timestamp(),
  `stok` int(255) DEFAULT 0,
  `tipe` int(250) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `admin_product`
--

INSERT INTO `admin_product` (`id`, `nama_produk`, `harga_jual`, `harga_kulak`, `tanggal_restock`, `stok`, `tipe`) VALUES
(143, 'telkomsel', 120000, 110000, '2025-05-16', 17, 100000);

-- --------------------------------------------------------

--
-- Table structure for table `transaksi`
--

CREATE TABLE `transaksi` (
  `id` int(11) NOT NULL,
  `nomor_tujuan` varchar(100) DEFAULT NULL,
  `product_id` int(90) DEFAULT NULL,
  `harga` varchar(100) DEFAULT NULL,
  `waktu_transaksi` timestamp NULL DEFAULT current_timestamp(),
  `quantity` int(100) DEFAULT NULL,
  `product` varchar(255) DEFAULT NULL,
  `kasir_username` varchar(100) DEFAULT NULL,
  `uang_pembeli` int(255) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `transaksi`
--

INSERT INTO `transaksi` (`id`, `nomor_tujuan`, `product_id`, `harga`, `waktu_transaksi`, `quantity`, `product`, `kasir_username`, `uang_pembeli`) VALUES
(42, '085755458450', 111, '50,000', '2025-05-09 06:33:57', 10, 'Indosat Paket D', 'vito', 0),
(43, '085755458450', 111, '50,000', '2025-05-09 06:34:33', 30, 'Indosat Paket D', 'vito', 0),
(44, '0895336412345', 119, '7,000', '2025-05-09 06:49:04', 20, 'tri paket A', 'hello', 0),
(45, '08961244124124', 118, '5,000', '2025-05-09 11:01:47', 10, 'byu', 'imron', 0),
(46, '0896872983176893', 118, '5,000', '2025-05-09 11:01:59', 100, 'byu', 'imron', 0),
(47, '081123121247091', 120, '10,000', '2025-05-09 11:05:27', 1, 'telkomsel', 'imron', 0),
(48, '081112312321321', 122, '100,000', '2025-05-14 03:25:46', 11, 'telkomsel', '0', 0),
(49, '081123812412', 122, '11000', '2025-05-14 03:50:24', 1, 'telkomsel', '0', 0),
(50, '081212834129848192', 122, '11000', '2025-05-14 03:56:44', 1, 'telkomsel', '0', 0),
(51, '0812898921489', 122, '11000', '2025-05-14 04:02:25', 1, 'telkomsel', '0', 19000),
(52, '081111241241234', 122, '11000', '2025-05-14 04:03:28', 1, 'telkomsel', '0', 11000),
(53, '0811123123123', 122, '11000', '2025-05-14 04:08:11', 1, 'telkomsel', '0', 11000),
(54, '0811212649812', 122, '11000', '2025-05-14 04:08:33', 1, 'telkomsel', '0', 12000),
(55, '0811214124', 122, '11000', '2025-05-14 04:10:53', 1, 'telkomsel', '0', 11000),
(56, '081122143123', 122, '11000', '2025-05-14 04:12:17', 1, 'telkomsel', '0', 100000),
(57, '08121241241234', 122, '11000', '2025-05-14 04:14:23', 1, 'telkomsel', '0', 1900001),
(58, '08957123721323213', 120, '11000', '2025-05-14 04:16:20', 1, 'tri', 'imron', 12000),
(59, '08951213766715276', 120, '11000', '2025-05-14 04:19:58', 1, 'tri', 'imron', 12000),
(60, '0895336412345', 120, '11000', '2025-05-14 04:51:03', 1, 'tri', 'imron', 100000),
(61, '089512399012', 135, '12000', '2025-05-14 11:44:16', 1, 'tri', '0', 13000),
(62, '0895141215271526712', 135, '7000', '2025-05-15 09:28:48', 1, 'tri', '0', 8000),
(63, '08951278587327819312', 135, '22000', '2025-05-16 02:21:52', 1, 'tri', 'imron', 23000),
(64, '08227147207109274091212412', 143, '120000', '2025-05-16 02:35:41', 1, 'telkomsel', 'imron', 123000),
(65, '08221271271239', 143, '120000', '2025-05-16 02:37:26', 1, 'telkomsel', 'imron', 150000),
(66, '0811123736217412423144', 143, '120000', '2025-05-18 00:14:49', 1, 'telkomsel', 'imron', 130000);

--
-- Indexes for dumped tables
--

--
-- Indexes for table `account`
--
ALTER TABLE `account`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `admin_product`
--
ALTER TABLE `admin_product`
  ADD PRIMARY KEY (`id`);

--
-- Indexes for table `transaksi`
--
ALTER TABLE `transaksi`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT for dumped tables
--

--
-- AUTO_INCREMENT for table `account`
--
ALTER TABLE `account`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=29;

--
-- AUTO_INCREMENT for table `admin_product`
--
ALTER TABLE `admin_product`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=144;

--
-- AUTO_INCREMENT for table `transaksi`
--
ALTER TABLE `transaksi`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=67;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
