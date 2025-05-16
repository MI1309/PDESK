-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Generation Time: May 14, 2025 at 02:53 AM
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
(25, 'hello', 'hello', 'kasir', 'jl.siwalan panji gang 3', '085755458450', 'pramodya@gmail.com', 'wisnu'),
(26, 'vito', 'vito', 'kasir', 'baskfbasukbfkjasbjlfnaslkfnlkafcanknflkas', '0809123892381', 'vito@gmail.co', 'vito hai');

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
(118, 'byu', 7000, 6000, '2025-05-09', 0, 5000),
(120, 'telkomsel', 11000, 10000, '2025-05-09', 9, 10000),
(122, 'indosat', 11000, 10000, '2025-05-09', 20, 100000);

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
  `kasir_username` varchar(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data for table `transaksi`
--

INSERT INTO `transaksi` (`id`, `nomor_tujuan`, `product_id`, `harga`, `waktu_transaksi`, `quantity`, `product`, `kasir_username`) VALUES
(42, '085755458450', 111, '50,000', '2025-05-09 06:33:57', 10, 'Indosat Paket D', 'vito'),
(43, '085755458450', 111, '50,000', '2025-05-09 06:34:33', 30, 'Indosat Paket D', 'vito'),
(44, '0895336412345', 119, '7,000', '2025-05-09 06:49:04', 20, 'tri paket A', 'hello'),
(45, '08961244124124', 118, '5,000', '2025-05-09 11:01:47', 10, 'byu', 'imron'),
(46, '0896872983176893', 118, '5,000', '2025-05-09 11:01:59', 100, 'byu', 'imron'),
(47, '081123121247091', 120, '10,000', '2025-05-09 11:05:27', 1, 'telkomsel', 'imron');

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
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=27;

--
-- AUTO_INCREMENT for table `admin_product`
--
ALTER TABLE `admin_product`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=127;

--
-- AUTO_INCREMENT for table `transaksi`
--
ALTER TABLE `transaksi`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=48;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
