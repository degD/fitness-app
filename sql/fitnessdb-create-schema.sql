
-- Kullanıcı (üye) bilgilerini saklar
CREATE TABLE "member" (
  "id" int PRIMARY KEY,		-- Yasal kimlik numarası
  "mail" varchar,
  "phone" varchar,
  "name" varchar,
  "weight" int,
  "height" int,
  "birth_date" date,
  "points" float,			-- Kullanıcının kazandığı indirim puanları
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

-- Kullanıcının belli bir seansa katılmak için oluşturduğu randevu.
CREATE TABLE "appointment" (
  "id" int PRIMARY KEY,
  "session_id" int REFERENCES "session"("id"),
  "member_id" int REFERENCES "member"("id"),
  "status" int,				-- Randevu durumu: completed, canceled, missed, scheduled
  "date" date
);

-- Egzersizler
CREATE TABLE "exercise" (
  "id" int PRIMARY KEY,
  "name" varchar
);

-- Kullanıcının önceden belirlediği egzersiz planı
CREATE TABLE "workout_plan" (
  "id" int PRIMARY KEY,
  "member_id" int REFERENCES "member"("id"),
  "exercise_id" int REFERENCES "exercise"("id"),
  "title" varchar,			-- Egzersiz planının uygulama içindeki adı
  "reps" int,				-- Repetations 
  "sets" int,				-- Set sayısı
  "weight" int,				-- Kaldırılan ağırlık, yoksa sıfır (0)
  "calories_burnt" int
);

-- Kullanıcı banka kartı. Bir kullanıcı birden çok kart tanımlayabilir.
-- Aynı numaralı kart da birden çok kullanıcı tarafından kullanılabilir. 
CREATE TABLE "card" (
  "title" varchar,			-- Kartın uygulama içinde görünecek adı
  "member_id" int REFERENCES "member"("id"),
  "number" int,				-- Kart numarası
  "card_owner" varchar,		-- Kart sahibi ad bilgileri
  "expire_date" date,		-- Kart SKT
  "cvv" int,
  PRIMARY KEY ("member_id", "number")
);

-- Kullanıcının yaptığı ödeme işlemleri geçmişini tutar. Kullanıcı, her seans
-- randevusu için ödeme yapar.
CREATE TABLE "transaction" (
  "member_id" int,
  "card_number" int,
  "invoice_id" int,			-- Yapılan işlemin fatura numarası (rasgele oluşturulacak)
  "total_amount" float,		-- Ödeme işleminin toplam fiyatı
  "points_used" float,		-- Bu fiyatın ödenmesinde ne kadar puan kullanıldığı
  "date" date,				-- Ödemenin gerçekleştiği tarih
  PRIMARY KEY ("member_id", "card_number"),
  FOREIGN KEY ("member_id", "card_number") REFERENCES "card"("member_id", "number") 
);

-- Kullanıcının bir seans için randevusu için seçtiği egzersiz planı
CREATE TABLE "appointed_workout_plan" (
  "appointment_id" int REFERENCES "appointment"("id"),
  "workout_plan_id" int REFERENCES "workout_plan"("id"),
  PRIMARY KEY ("appointment_id", "workout_plan_id")
);
