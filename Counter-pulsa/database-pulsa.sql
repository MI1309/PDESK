-- phpMyAdmin SQL Dump
-- version 5.2.1
-- https://www.phpmyadmin.net/
--
-- Host: 127.0.0.1
-- Waktu pembuatan: 26 Apr 2025 pada 11.05
-- Versi server: 10.4.28-MariaDB
-- Versi PHP: 8.2.4

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
-- Struktur dari tabel `account`
--

CREATE TABLE `account` (
  `id` int(11) NOT NULL,
  `username` varchar(30) NOT NULL,
  `password` varchar(30) NOT NULL,
  `role` varchar(30) NOT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_general_ci;

--
-- Dumping data untuk tabel `account`
--

INSERT INTO `account` (`id`, `username`, `password`, `role`) VALUES
(1, 'admin', 'admin', 'admin'),
(6, 'amir', 'amir', 'kasir'),
(7, 'imron', 'imron', 'kasir');

-- --------------------------------------------------------

--
-- Struktur dari tabel `admin_product`
--

CREATE TABLE `admin_product` (
  `id` int(11) NOT NULL,
  `nama_produk` varchar(100) DEFAULT NULL,
  `harga_jual` int(11) DEFAULT NULL,
  `harga_modal` int(11) NOT NULL,
  `tanggal_restock` date NOT NULL DEFAULT current_timestamp(),
  `stok` int(11) DEFAULT 0,
  `id_kasir` int(11) DEFAULT NULL,
  `kasir_username` varchar(50) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `admin_product`
--

INSERT INTO `admin_product` (`id`, `nama_produk`, `harga_jual`, `harga_modal`, `tanggal_restock`, `stok`, `id_kasir`, `kasir_username`) VALUES
(55, 'tri', 132, 2, '2025-04-26', 1381, 6, 'amir'),
(56, 'telkomsel', 12, 12, '2025-04-26', 12, 6, 'amir');

-- --------------------------------------------------------

--
-- Struktur dari tabel `transaksi`
--

CREATE TABLE `transaksi` (
  `id` int(11) NOT NULL,
  `nama_pembeli` varchar(100) DEFAULT NULL,
  `nomor_tujuan` varchar(100) DEFAULT NULL,
  `product_id` varchar(1) DEFAULT NULL,
  `harga` varchar(100) DEFAULT NULL,
  `waktu_transaksi` timestamp NULL DEFAULT current_timestamp(),
  `quantity` int(100) DEFAULT NULL
) ENGINE=InnoDB DEFAULT CHARSET=utf8mb4 COLLATE=utf8mb4_unicode_ci;

--
-- Dumping data untuk tabel `transaksi`
--

INSERT INTO `transaksi` (`id`, `nama_pembeli`, `nomor_tujuan`, `product_id`, `harga`, `waktu_transaksi`, `quantity`) VALUES
(1, 'ASF', '131', 'x', '1028901', '2025-04-26 08:42:07', 2343),
(2, 'ad', '131', '6', '1028901', '2025-04-26 08:51:08', 12),
(3, 'ad', '131', '6', '1028901', '2025-04-26 08:51:24', 1200),
(4, 'ad', '131', '6', '1028901', '2025-04-26 08:51:28', 1200),
(5, 'asep', '109281', '6', '100000', '2025-04-26 08:54:06', 10),
(6, 'asep', '109281', '6', '100000', '2025-04-26 08:54:16', 10),
(7, 'admi', '131241', '5', '1', '2025-04-26 08:55:24', 10000),
(8, '131asf', '12412', '5', '1', '2025-04-26 08:57:34', 1);

--
-- Indexes for dumped tables
--

--
-- Indeks untuk tabel `account`
--
ALTER TABLE `account`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `admin_product`
--
ALTER TABLE `admin_product`
  ADD PRIMARY KEY (`id`);

--
-- Indeks untuk tabel `transaksi`
--
ALTER TABLE `transaksi`
  ADD PRIMARY KEY (`id`);

--
-- AUTO_INCREMENT untuk tabel yang dibuang
--

--
-- AUTO_INCREMENT untuk tabel `account`
--
ALTER TABLE `account`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=8;

--
-- AUTO_INCREMENT untuk tabel `admin_product`
--
ALTER TABLE `admin_product`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=62;

--
-- AUTO_INCREMENT untuk tabel `transaksi`
--
ALTER TABLE `transaksi`
  MODIFY `id` int(11) NOT NULL AUTO_INCREMENT, AUTO_INCREMENT=9;
COMMIT;

/*!40101 SET CHARACTER_SET_CLIENT=@OLD_CHARACTER_SET_CLIENT */;
/*!40101 SET CHARACTER_SET_RESULTS=@OLD_CHARACTER_SET_RESULTS */;
/*!40101 SET COLLATION_CONNECTION=@OLD_COLLATION_CONNECTION */;
