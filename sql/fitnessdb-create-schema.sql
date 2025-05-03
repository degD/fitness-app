
-- Kullanıcı (üye) bilgilerini saklar
CREATE TABLE "member" (
  "id" varchar PRIMARY KEY,		-- Yasal kimlik numarası
  "mail" varchar,
  "phone" varchar,
  "name" varchar,
  "weight" int,
  "height" int,
  "birth_date" date,
  "points" float,			-- Kullanıcının kazandığı indirim puanları
  "type" int,         -- Kullanıcı tipi, 0: normal, 1: admin
  "password" varchar
);

-- Antrenörler
CREATE TABLE "trainer" (
  "id" int PRIMARY KEY,
  "name" varchar,
  "phone" varchar
);

-- Kullanıcının randevu oluşturarak fitness salonunu kullanabileceği, belli 
-- saatler arasındaki seanslar. Seansların kapasiteleri ve türleri vardır.
-- Türlere örnek olarak grup seansları, kişisel seanslar, antrenör eşliğindeki
-- seanslar verilebilir. 
CREATE TABLE "session" (
  "id" int PRIMARY KEY,
  "type" int,				-- Seans tipi
  "total_capacity" int,
  "current_capacity" int,
  "status" int,
  "date" date,
  "start" time,
  "end" time,
  "trainer_id" int REFERENCES "trainer"("id")
  		-- Seans ile ilişkilendirilmiş antrenör ID'si. Bireysel seanslar için null.
);

-- Egzersizler
CREATE TABLE "exercise" (
  "id" int PRIMARY KEY,
  "name" varchar
);

-- Kullanıcının önceden belirlediği egzersiz planı
CREATE TABLE "workout_plan" (
  "id" int PRIMARY KEY,
  "title" varchar,			-- Egzersiz planının uygulama içindeki adı
  "member_id" varchar REFERENCES "member"("id")
);

-- Egzersiz planında bulunan egzersizleri ve kaç kere yapılacaklarının bilgisini tutar.
CREATE TABLE "workout_plan_exercise" (
  "workout_plan_id" int REFERENCES "workout_plan"("id"),	
  "exercise_id" int REFERENCES "exercise"("id"),
  "reps" int,				-- Repetations 
  "sets" int,				-- Set sayısı
  "weight" int,				-- Kaldırılan ağırlık, yoksa sıfır (0)
  "calories_burnt" int,		-- Tahmini yakılacak  kalori miktarı
  PRIMARY KEY ("workout_plan_id", "exercise_id")
);

-- Kullanıcı banka kartı. Bir kullanıcı birden çok kart tanımlayabilir.
-- Aynı numaralı kart da birden çok kullanıcı tarafından kullanılabilir. 
CREATE TABLE "card" (
  "title" varchar,			-- Kartın uygulama içinde görünecek adı
  "member_id" varchar REFERENCES "member"("id"),
  "number" varchar,			-- Kart numarası
  "card_owner" varchar,		-- Kart sahibi ad bilgileri
  "expire_date" date,		-- Kart SKT
  "cvv" int,
  PRIMARY KEY ("member_id", "number")
);

-- Kullanıcının yaptığı ödeme işlemleri geçmişini tutar. Kullanıcı, her seans
-- randevusu için ödeme yapar.
CREATE TABLE "transaction" (
  "member_id" varchar,
  "card_number" varchar,
  "invoice_id" int PRIMARY KEY,	-- Yapılan işlemin fatura numarası (rasgele oluşturulacak)
  "total_amount" float,				-- Ödeme işleminin toplam fiyatı
  "points_used" float,				-- Bu fiyatın ödenmesinde ne kadar puan kullanıldığı
  "date" date,						-- Ödemenin gerçekleştiği tarih
  FOREIGN KEY ("member_id", "card_number") REFERENCES "card"("member_id", "number") 
);

-- Kullanıcının belli bir seansa katılmak için oluşturduğu randevu.
CREATE TABLE "appointment" (
  "id" int PRIMARY KEY,
  "session_id" int REFERENCES "session"("id"),
  "member_id" varchar REFERENCES "member"("id"),
  "status" int,				-- Randevu durumu: completed, canceled, missed, scheduled
  "date" date,
  "workout_plan_id" int REFERENCES "workout_plan"("id")
  	-- Kullanıcının bir randevusu için seçtiği egzersiz planı
);

-- Admin'in giriş şifresini hashlenmiş halde tutan tablo
CREATE TABLE admin_password (
  id BOOLEAN PRIMARY KEY DEFAULT TRUE CHECK (id), -- always TRUE, only one row can exist
  password_hash TEXT NOT NULL,
  created_at TIMESTAMP DEFAULT NOW()
);
INSERT INTO admin_password (id, password_hash) 
VALUES (TRUE, '$2a$11$TR7nmf6dw4RQRLqeg3dJ0.8TgfUBwRePM4nv7lURdpAkQD0Co/iEi');
-- Başlangıç admin şifresi: 'password'
-- Bundan sonra yeni INSERT yapılamaz. Yalnız UPDATE.

/*
// TODO: ödenen kısım mı sadece puan olsun yoksa toplam mı?
// kullanıcı yaptığı harcama kadar puan kazanır. 
// (100TL harcama -> 100 puan kazandırır) 
// ama 1 puan 5 kuruş gibi indirim yapar
// kullanıcı puan kullanmayı seçerse önce tüm puanlar
// kullanılır sonra kalan tutarın ödemesi yapılır

// kalori hesabı
// vücut kitle indeksi
// ne kadar hangi egzersiz chart
// vs...
*/
