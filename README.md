# DOTRestAPI
Untuk PT. Digdaya Olah Teknologi (DOT) Indonesia.


Berikut adalah contoh **dokumentasi REST API** untuk **Categories** dan **Products** sesuai dengan kode controller yang telah dimodifikasi (beserta validasi, response sukses, dan penanganan error). Contoh ini berbentuk **teks** yang umum dipakai di README atau dokumentasi internal. Silakan sesuaikan lebih lanjut sesuai kebutuhan (misalnya menambahkan parameter query tertentu, menulis format **Swagger/OpenAPI** formal, dsb.).

---

# **Dokumentasi REST API**

## **Pendahuluan**

API ini menyediakan layanan untuk mengelola data **Categories** dan **Products** dalam sebuah sistem. Terdapat dua resource utama:

1. **Categories**  
   Endpoint dasar: `/api/Categories`
2. **Products**  
   Endpoint dasar: `/api/Products`

Setiap resource memiliki operasi **CRUD** (Create, Read, Update, Delete).

### **Struktur Response Umum**

Setiap response dari endpoint umumnya mengembalikan struktur JSON dengan format:

```json
{
  "success": true/false,
  "message": "Deskripsi singkat",
  "data": ... (opsional, hanya jika relevan)
}
```

- **`success`**: Menandakan apakah operasi berhasil (`true`) atau gagal (`false`).
- **`message`**: Keterangan singkat atau detail mengenai hasil eksekusi.
- **`data`**: Objek atau array data (misalnya data Category, data Product) jika operasi mengharuskan pengembalian data.

Ketika terjadi error atau gagal validasi, status code akan menyesuaikan (contoh `400 Bad Request`, `404 Not Found`, atau `500 Internal Server Error`) dan `success` akan bernilai `false`.

---

## **I. Categories Endpoint**

### **1. GET /api/Categories**

- **Deskripsi**: Mengambil daftar seluruh kategori.  
- **Eager Loading**: Termasuk data `Products` di dalamnya.  

**Contoh Permintaan (Request)**:
```
GET /api/Categories
```

**Contoh Respon (Response) - 200 OK**:
```json
{
  "success": true,
  "message": "Data kategori berhasil diambil",
  "data": [
    {
      "id": 1,
      "name": "Electronics",
      "products": [
        {
          "id": 10,
          "name": "Smart TV",
          "price": 549.00,
          "categoryId": 1
        },
        ...
      ]
    },
    ...
  ]
}
```

**Catatan**:  
- `data` berisi array dari kategori.  
- Masing-masing kategori memuat properti `products` (List of Products).

---

### **2. GET /api/Categories/{id}**

- **Deskripsi**: Mengambil data kategori tertentu berdasarkan `id`.  
- **Termasuk** data product di dalam `Category.products`.  

**Contoh Permintaan**:
```
GET /api/Categories/5
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Data kategori berhasil diambil",
  "data": {
    "id": 5,
    "name": "Cameras",
    "products": [
      {
        "id": 101,
        "name": "Digital Camera",
        "price": 249.00,
        "categoryId": 5
      }
    ]
  }
}
```

**Contoh Respon - 404 Not Found**:
```json
{
  "success": false,
  "message": "Category dengan id 999 tidak ditemukan."
}
```

---

### **3. POST /api/Categories**

- **Deskripsi**: Membuat data kategori baru.  

**Header yang disarankan**:
```
Content-Type: application/json
```

**Contoh Body**:
```json
{
  "name": "New Category"
}
```

**Contoh Respon - 201 Created**:
```json
{
  "success": true,
  "message": "Category berhasil disimpan",
  "data": {
    "id": 51,
    "name": "New Category",
    "products": []
  }
}
```
- Mengembalikan objek kategori yang baru dibuat.

**Contoh Respon - 400 Bad Request (Validasi Gagal)**:
```json
{
  "success": false,
  "message": "Data kategori tidak valid",
  "errors": [
    "Nama kategori wajib diisi"
  ]
}
```

---

### **4. PUT /api/Categories/{id}**

- **Deskripsi**: Mengupdate data kategori dengan `id` tertentu.  

**Contoh Body**:
```json
{
  "id": 5,
  "name": "Updated Category"
}
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Category dengan id 5 berhasil di-update",
  "data": {
    "id": 5,
    "name": "Updated Category"
  }
}
```

**Contoh Respon - 404 Not Found**:
```json
{
  "success": false,
  "message": "Category dengan id 999 tidak ditemukan."
}
```

---

### **5. DELETE /api/Categories/{id}**

- **Deskripsi**: Menghapus data kategori berdasarkan `id`.  

**Contoh Permintaan**:
```
DELETE /api/Categories/5
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Category dengan id 5 berhasil dihapus."
}
```

**Contoh Respon - 404 Not Found**:
```json
{
  "success": false,
  "message": "Category dengan id 999 tidak ditemukan."
}
```

---

## **II. Products Endpoint**

### **1. GET /api/Products**

- **Deskripsi**: Mengambil daftar semua produk.  
- **Eager loading**: Memuat data kategori (`Category`) di dalam product.  

**Contoh Permintaan**:
```
GET /api/Products
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Data produk berhasil diambil",
  "data": [
    {
      "id": 10,
      "name": "Smart TV",
      "price": 549.00,
      "categoryId": 1,
      "category": {
        "id": 1,
        "name": "Electronics"
      }
    },
    ...
  ]
}
```

---

### **2. GET /api/Products/{id}**

- **Deskripsi**: Mengambil data produk dengan `id` tertentu.  

**Contoh Permintaan**:
```
GET /api/Products/10
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Data produk berhasil diambil",
  "data": {
    "id": 10,
    "name": "Smart TV",
    "price": 549.00,
    "categoryId": 1,
    "category": {
      "id": 1,
      "name": "Electronics"
    }
  }
}
```

**Contoh Respon - 404 Not Found**:
```json
{
  "success": false,
  "message": "Produk dengan id 999 tidak ditemukan."
}
```

---

### **3. POST /api/Products**

- **Deskripsi**: Membuat data produk baru.  

**Header yang disarankan**:
```
Content-Type: application/json
```

**Contoh Body**:
```json
{
  "name": "Laptop Gaming",
  "price": 1200.00,
  "categoryId": 2
}
```

**Contoh Respon - 201 Created**:
```json
{
  "success": true,
  "message": "Produk berhasil disimpan",
  "data": {
    "id": 55,
    "name": "Laptop Gaming",
    "price": 1200.0,
    "categoryId": 2
  }
}
```

**Contoh Respon - 400 Bad Request (Validasi Gagal)**:
```json
{
  "success": false,
  "message": "Data produk tidak valid",
  "errors": [
    "CategoryId tidak ditemukan"
  ]
}
```
- Misalnya ketika `CategoryId` tidak ada di tabel Categories.

---

### **4. PUT /api/Products/{id}**

- **Deskripsi**: Mengupdate data produk dengan `id` tertentu.  

**Contoh Body**:
```json
{
  "id": 55,
  "name": "Laptop Gaming Pro",
  "price": 1300.00,
  "categoryId": 2
}
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Produk dengan id 55 berhasil di-update",
  "data": {
    "id": 55,
    "name": "Laptop Gaming Pro",
    "price": 1300.0,
    "categoryId": 2
  }
}
```

**Contoh Respon - 404 Not Found**:
```json
{
  "success": false,
  "message": "Produk dengan id 999 tidak ditemukan."
}
```

**Contoh Respon - 400 Bad Request (Category tidak valid)**:
```json
{
  "success": false,
  "message": "Data produk tidak valid",
  "errors": [
    "CategoryId tidak ditemukan"
  ]
}
```

---

### **5. DELETE /api/Products/{id}**

- **Deskripsi**: Menghapus data produk berdasarkan `id`.  

**Contoh Permintaan**:
```
DELETE /api/Products/55
```

**Contoh Respon - 200 OK**:
```json
{
  "success": true,
  "message": "Produk dengan id 55 berhasil dihapus."
}
```

**Contoh Respon - 404 Not Found**:
```json
{
  "success": false,
  "message": "Produk dengan id 999 tidak ditemukan."
}
```

---

## **Error Handling Tambahan**

Selain contoh-contoh di atas, jika terjadi **exception** tak terduga (misal kegagalan koneksi database, error internal), server akan mengembalikan **HTTP 500** dengan struktur JSON seperti:

```json
{
  "success": false,
  "message": "Terjadi kesalahan: <pesan error>"
}
```

---

## **Kesimpulan**

1. **Endpoint**:
   - **Categories**: `/api/Categories`
   - **Products**: `/api/Products`

2. **Operation**:
   - **GET** (All / By ID)  
   - **POST** (Create)  
   - **PUT** (Update)  
   - **DELETE** (Delete)

3. **Format Respon**:
   - Sukses: `success = true`, `message`, dan `data` (jika relevan).  
   - Gagal: `success = false`, `message` error, `errors` (untuk detail validasi), serta status code yang sesuai (`400`, `404`, `500`, dsb.).

4. **Validasi**:
   - Cek model (`ModelState`) di sisi server.  
   - Cek eksistensi `CategoryId` saat membuat/meng-update produk.  

Dokumentasi ini menggambarkan secara umum cara memanggil API, struktur data request/response, dan contoh-contoh output yang diharapkan. Silakan disesuaikan dengan **kebutuhan bisnis** atau **konvensi** yang diinginkan.
