INSERT INTO "member" VALUES
('00000000000', 'admin@example.com', '0000000000', 'admin', 0, 0, '1960-01-01', 0.0, 1, 'admin'),
('12345678901', 'ali@example.com', '5551234567', 'Ali Yılmaz', 75, 180, '1990-05-15', 120.5, 0, 'password123'),
('23456789012', 'ayse@example.com', '5309876543', 'Ayşe Demir', 60, 165, '1995-08-22', 95.0, 0, 'securepass'),
('34567890123', 'mehmet@example.com', '5324567890', 'Mehmet Kara', 85, 175, '1987-11-10', 45.5, 0, 'mehmetpass'),
('45678901234', 'zeynep@example.com', '5553456789', 'Zeynep Çelik', 55, 160, '2000-02-05', 150.0, 0, 'zeynep123'),
('56789012345', 'omer@example.com', '5401239876', 'Ömer Aslan', 78, 170, '1992-07-30', 30.0, 0, 'omerpass'),
('67890123456', 'elif@example.com', '5337654321', 'Elif Şahin', 65, 168, '1998-04-12', 200.0, 0, 'elifsecure'),
('78901234567', 'burak@example.com', '5559871234', 'Burak Taş', 90, 185, '1985-09-18', 75.0, 0, 'burakpass'),
('89012345678', 'fatma@example.com', '5302345678', 'Fatma Öz', 58, 162, '1996-12-25', 180.5, 0, 'fatmapass'),
('90123456789', 'hakan@example.com', '5326789012', 'Hakan Yıldız', 80, 178, '1991-03-14', 60.0, 0, 'hakan123'),
('01234567890', 'emine@example.com', '5558765432', 'Emine Güneş', 63, 166, '1994-06-28', 220.0, 0, 'eminepass');

INSERT INTO "trainer" VALUES
(1, 'Ahmet Kaya', '5551112233'),
(2, 'Merve Yıldırım', '5309876543'),
(3, 'Burak Demir', '5324567890'),
(4, 'Zeynep Aydın', '5553456789'),
(5, 'Hakan Şahin', '5401239876');

INSERT INTO "session" VALUES
(1, 1, 10, 5, 1, '2025-03-20', '10:00:00', '11:00:00', 1),		 -- Grup seansı (Ahmet Kaya)
(2, 2, 1, 1, 1, '2025-03-21', '14:00:00', '15:00:00', NULL), 	 -- Bireysel seans
(3, 3, 5, 2, 1, '2025-03-22', '16:00:00', '17:30:00', 3),  	 -- Antrenörlü seans (Burak Demir)
(4, 1, 8, 8, 0, '2025-03-23', '09:30:00', '10:30:00', 2),  	 -- Dolu grup seansı (Merve Yıldırım)
(5, 3, 6, 3, 1, '2025-03-24', '18:00:00', '19:30:00', 5);  	 -- Antrenörlü seans (Hakan Şahin)
-- 1 (Grup Seansı), 2 (Bireysel Seans), 3 (Antrenörlü Seans)
-- 1 (Aktif), 0 (Dolu)

INSERT INTO "card" ("title", "member_id", "number", "card_owner", "expire_date", "cvv") VALUES
('Kredi Kartım', '12345678901', '1111222233334444', 'Ali Yılmaz', '2027-08-31', 123),
('Alışveriş Kartı', '23456789012', '5555666677778888', 'Ayşe Demir', '2026-05-30', 456),
('Sanal Kart', '34567890123', '9999000011112222', 'Mehmet Kara', '2028-12-15', 789),
('Fitness Kartı', '45678901234', '1111222233334444', 'Zeynep Çelik', '2027-08-31', 123), -- Aynı kart numarası farklı kullanıcıda
('Yedek Kart', '56789012345', '3333444455556666', 'Ömer Aslan', '2025-10-20', 321);

INSERT INTO "exercise" ("id", "name") VALUES
(1, 'Squat'),
(2, 'Deadlift'),
(3, 'Push-up'),
(4, 'Pull-up'),
(5, 'Bench Press'),
(6, 'Lunges'),
(7, 'Plank'),
(8, 'Leg Press'),
(9, 'Bicep Curl'),
(10, 'Tricep Dip'),
(11, 'Dumbbell Row'),
(12, 'Shoulder Press'),
(13, 'Barbell Row'),
(14, 'Leg Curl'),
(15, 'Hip Thrust'),
(16, 'Russian Twist'),
(17, 'Mountain Climbers'),
(18, 'Kettlebell Swing'),
(19, 'Ab Roll-out'),
(20, 'Calf Raise');

INSERT INTO "workout_plan" ("id", "title", "member_id") VALUES
(1, 'Leg Day', '12345678901'),  		   -- Ali Yılmaz'ın planı
(2, 'Chest Strength', '23456789012'),     -- Ayşe Demir'in planı
(3, 'Arms Workout', '34567890123'),	   -- Mehmet Kara'nın planı
(4, 'Full Body', '45678901234'),  		   -- Zeynep Çelik'in planı
(5, 'Shoulder Training', '56789012345');  -- Ömer Aslan'ın planı

INSERT INTO "workout_plan_exercise" ("workout_plan_id", "exercise_id", "reps", "sets", "weight", "calories_burnt") VALUES
-- Ali Yılmaz'ın 'Leg Day' planı için egzersizler
(1, 1, 12, 4, 60, 400),  -- Squat
(1, 8, 15, 4, 70, 350),  -- Leg Press
(1, 6, 12, 3, 0, 200),   -- Lunges
(1, 7, 30, 3, 0, 150),   -- Plank
(1, 10, 20, 4, 0, 100),  -- Calf Raise

-- Ayşe Demir'in 'Chest Strength' planı için egzersizler
(2, 5, 10, 3, 40, 300),  -- Bench Press
(2, 9, 12, 3, 15, 200),  -- Bicep Curl
(2, 3, 20, 4, 0, 250),   -- Push-up
(2, 13, 12, 3, 20, 220); -- Barbell Row

INSERT INTO "appointment" ("id", "session_id", "member_id", "status", "date", "workout_plan_id") VALUES
(1, 1, '12345678901', 1, '2025-03-20', 1),  -- Ali Yılmaz, grup seansı (Leg Day planı)
(2, 2, '23456789012', 0, '2025-03-21', 2),  -- Ayşe Demir, bireysel seans (Chest Strength planı, canceled)
(3, 3, '34567890123', 1, '2025-03-22', 3),  -- Mehmet Kara, antrenörlü seans (Arms Workout planı)
(4, 5, '56789012345', 1, '2025-03-24', 5);  -- Ömer Aslan, antrenörlü seans (Shoulder Training planı)

INSERT INTO "transaction" ("member_id", "card_number", "invoice_id", "total_amount", "points_used", "date") VALUES
('12345678901', '1111222233334444', 1001, 150.0, 50.0, '2025-03-20'),   -- Ali Yılmaz, Kredi Kartı
('23456789012', '5555666677778888', 1002, 200.0, 80.0, '2025-03-21'),   -- Ayşe Demir, Alışveriş Kartı
('34567890123', '9999000011112222', 1003, 120.0, 40.0, '2025-03-22'),   -- Mehmet Kara, Sanal Kart
('45678901234', '1111222233334444', 1004, 180.0, 60.0, '2025-03-23'),   -- Zeynep Çelik, Kredi Kartı
('56789012345', '3333444455556666', 1005, 250.0, 100.0, '2025-03-24'),  -- Ömer Aslan, Yedek Kart
('12345678901', '1111222233334444', 1006, 160.0, 60.0, '2025-03-25'),   -- Ali Yılmaz, Kredi Kartı
('23456789012', '5555666677778888', 1007, 220.0, 90.0, '2025-03-26'),   -- Ayşe Demir, Alışveriş Kartı
('34567890123', '9999000011112222', 1008, 140.0, 50.0, '2025-03-27'),   -- Mehmet Kara, Sanal Kart
('45678901234', '1111222233334444', 1009, 190.0, 70.0, '2025-03-28'),   -- Zeynep Çelik, Kredi Kartı
('56789012345', '3333444455556666', 1010, 230.0, 110.0, '2025-03-29'),  -- Ömer Aslan, Yedek Kart
('12345678901', '1111222233334444', 1100, 150.0, 50.0, '2025-03-20'),   -- Ali Yılmaz, Kredi Kartı
('12345678901', '1111222233334444', 1200, 150.0, 50.0, '2025-03-20'),   -- Ali Yılmaz, Kredi Kartı
('12345678901', '1111222233334444', 1300, 150.0, 50.0, '2025-03-20'),   -- Ali Yılmaz, Kredi Kartı

UPDATE member SET password = '$2a$11$DQqeqBSKjP1LVjZrWTpcoOa1ORZuJZExhI2sT8oyDcBjxqx0dvh1i' WHERE id = '12345678901';
UPDATE member SET password = '$2a$11$zNmjIEMMvLKXda6pVKvSn.hAi4OqEVVJbhM6t8q1WGC5LJFEAoIFC' WHERE id = '23456789012';
UPDATE member SET password = '$2a$11$E4P88RPuO1aL3p9wpuImqOWVOAxYc7Tg0wQFdkFxLxWBKCOsKi/fO' WHERE id = '34567890123';
UPDATE member SET password = '$2a$11$Vr9MQ1RRSYjGTUax.XN5suDEz3NHEJkVR44LzAJS4d2nQ3LVcXWI2' WHERE id = '45678901234';
UPDATE member SET password = '$2a$11$6IT3YZsn.fhhqfwzGqgZKu4RHO4jqCEdVDmEQ9wT9.vcfpjpkfPWq' WHERE id = '56789012345';
UPDATE member SET password = '$2a$11$DrUFG65fOBsf1xi5xDwTne89N7yXtDMKYpsW6OWgJIT1ILAJZ5mDq' WHERE id = '67890123456';
UPDATE member SET password = '$2a$11$Qy3SeIfmFr4keFSfq51ScOSs9ARrM18C5j6p0ejrLrBp5Mw43ksvO' WHERE id = '78901234567';
UPDATE member SET password = '$2a$11$MREj3xJCrVXsbATelvP7heR2Pnn7ebNj3VOet9L0oZp1NDYyEtRuC' WHERE id = '89012345678';
UPDATE member SET password = '$2a$11$OBofYBWn8QMM2raYq3XY1.kqT7n9FKRM3aBQfp2JglXRwAilkrGjq' WHERE id = '90123456789';
UPDATE member SET password = '$2a$11$wrfUdsNW8cc0E4IJdwT91eWAp8AzU7FF33KZLZsfBmvWOfWDVxF.O' WHERE id = '01234567890';

ALTER TABLE member ALTER COLUMN password TYPE TEXT;




