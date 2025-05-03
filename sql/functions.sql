-- Return true if user with id exists
CREATE OR REPLACE FUNCTION user_exists(pid TEXT)
RETURNS BOOLEAN AS $$
BEGIN
    RETURN EXISTS (
        SELECT 1 FROM "member" WHERE "id" = pid
    );
END;
$$ LANGUAGE plpgsql;


