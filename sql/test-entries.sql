INSERT INTO "member" VALUES
('1231', 'ali@example.com', 	'5551234567', 'Ali Yılmaz', 	75, 180, '1990-05-15', 'Erkek', 	0, '$2a$12$jUkW2yjAj4yHPBKAUy4jCOSU2xhD7tqDoQSm3imSF9brXA97esjja'),
('1232', 'ayse@example.com', 	'5309876543', 'Ayşe Demir', 	60, 165, '1995-08-22', 'Kadın', 	0, '$2a$12$3RJNfkv8dwD9SGjzr1B9BuMlxMI75jHD3d8CIs0U78D3a/D7nFz2i'),
('1233', 'mehmet@example.com', '5324567890', 'Mehmet Kara', 	85, 175, '1987-11-10', 'Erkek', 	0, '$2a$12$nm8AWb8Xw60MOe0wKY5pNecXF9lJg6KzMiS7f50zcUitOrepsAgk.'),
('1234', 'zeynep@example.com', '5553456789', 'Zeynep Çelik', 	55, 160, '2000-02-05', 'Kadın',	0, '$2a$12$QwXTewzeo6.7tl4Jiwr6z.uGw9NtztP0jSfYDCfJiPZdaDgTMoeWS'),
('1235', 'omer@example.com', 	'5401239876', 'Ömer Aslan', 	78, 170, '1992-07-30', 'Erkek',	0, '$2a$12$f3miDFWXN9pKoNM8cUP22OuxbWhnMT3pPt4H0vu2g3i2Vpw7SiCxW');

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
('Kredi Kartım', '1231', '1111222233334444', 'Ali Yılmaz', '2027-08-31', 123),
('Alışveriş Kartı', '1231', '5555666677778888', 'Ayşe Demir', '2026-05-30', 456),
('Sanal Kart', '1232', '9999000011112222', 'Mehmet Kara', '2028-12-15', 789),
('Fitness Kartı', '1233', '1111222233334444', 'Zeynep Çelik', '2027-08-31', 123), -- Aynı kart numarası farklı kullanıcıda
('Yedek Kart', '1234', '3333444455556666', 'Ömer Aslan', '2025-10-20', 321);

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
(1, 'Leg Day', '1231'),  		   -- Ali Yılmaz'ın planı
(2, 'Chest Strength', '1231'),     -- Ayşe Demir'in planı
(3, 'Arms Workout', '1233'),	   -- Mehmet Kara'nın planı
(4, 'Full Body', '1234'),  		   -- Zeynep Çelik'in planı
(5, 'Shoulder Training', '1235');  -- Ömer Aslan'ın planı

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
(1, 1, '1231', 1, '2025-03-20', 1),  -- Ali Yılmaz, grup seansı (Leg Day planı)
(2, 2, '1232', 0, '2025-03-21', 2),  -- Ayşe Demir, bireysel seans (Chest Strength planı, canceled)
(3, 3, '1233', 1, '2025-03-22', 3),  -- Mehmet Kara, antrenörlü seans (Arms Workout planı)
(4, 5, '1234', 1, '2025-03-24', 5);  -- Ömer Aslan, antrenörlü seans (Shoulder Training planı)

INSERT INTO "transaction" ("member_id", "card_number", "invoice_id", "total_amount", "points_used", "date") VALUES
('1231', '1111222233334444', 1001, 150.0, 50.0, '2025-03-20'),
('1231', '5555666677778888', 1002, 200.0, 80.0, '2025-03-21'),
('1232', '9999000011112222', 1003, 120.0, 40.0, '2025-03-22'),
('1231', '1111222233334444', 1004, 180.0, 60.0, '2025-03-23'),
('1234', '3333444455556666', 1005, 250.0, 100.0, '2025-03-24'),
('1231', '1111222233334444', 1006, 160.0, 60.0, '2025-03-25'),
('1231', '5555666677778888', 1007, 220.0, 90.0, '2025-03-26'),
('1232', '9999000011112222', 1008, 140.0, 50.0, '2025-03-27'),
('1231', '1111222233334444', 1009, 190.0, 70.0, '2025-03-28'),
('1234', '3333444455556666', 1010, 230.0, 110.0, '2025-03-29'),
('1231', '1111222233334444', 1100, 150.0, 50.0, '2025-03-20'),
('1231', '1111222233334444', 1200, 150.0, 50.0, '2025-03-20'),
('1231', '1111222233334444', 1300, 150.0, 50.0, '2025-03-20');

INSERT INTO "exercise" ("id", "name") VALUES
(21, 'Dumbell Press'),
(22, 'Dumbell Incline Press'),
(23, 'Machine Row'),
(24, 'T-Row'),
(25, 'Lateral Raise'),
(26, 'Machine Press'),
(27, 'Dumbell Curl'),
(28, 'Triceps Pushdown'),
(29, 'Rope Pushdown');





