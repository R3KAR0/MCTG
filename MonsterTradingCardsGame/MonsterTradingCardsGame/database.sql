COMMENT ON DATABASE "MCTG"
    IS 'MonsterCardTradingGame';
	
CREATE TABLE users (
    u_id        char(36) PRIMARY KEY,
    username    varchar(40) NOT NULL UNIQUE,
    u_password  varchar(64) NOT NULL,
    coins   	integer NOT NULL,
	u_description varchar(2048),
	picture 	bytea 
);

CREATE TYPE e_kind AS ENUM ('spell', 'goblin', 'dragon', 'orc', 'knight', 'kraken', 'elves');
CREATE TYPE e_type AS ENUM ('spell', 'monster');
CREATE TYPE e_element AS ENUM ('fire', 'water', 'neutral');

CREATE TABLE packages (
    p_id        	char(36) PRIMARY KEY,
    p_description 	varchar(2048) NOT NULL,
	creationtime 	timestamp NOT NULL,
	price			int NOT NULL,
	buyer			char(36) NOT NULL,
	CONSTRAINT fk_packages_user FOREIGN KEY(buyer) REFERENCES users(u_id)
);

CREATE TABLE cards (
    c_id        char(36) PRIMARY KEY,
    c_description varchar(2048) NOT NULL,
	c_kind		e_kind NOT NULL,
	c_type		e_type NOT NULL,
	c_element	e_element NOT NULL,
	creationtime timestamp NOT NULL,
	damage integer NOT NULL,
	u_id	char(36) NOT NULL,
	p_id 	char(36) NOT NULL,
	CONSTRAINT fk_card_user FOREIGN KEY(u_id) REFERENCES users(u_id),
	CONSTRAINT fk_card_package FOREIGN KEY(p_id) REFERENCES packages(p_id) 
);

CREATE TABLE decks (
	d_id 			char(36) PRIMARY KEY,
	d_description 	char(2048) NOT NULL,
	creationtime 	timestamp NOT NULL,
	u_id			char(36) NOT NULL,
	CONSTRAINT fk_stack_user FOREIGN KEY(u_id) REFERENCES users(u_id) 
);

CREATE TABLE battleResults (
    br_id       char(36) PRIMARY KEY,
    user1		char(36) NOT NULL,
	CONSTRAINT user1_br FOREIGN KEY(user1) REFERENCES users(u_id),
	user2		char(36) NOT NULL,
	CONSTRAINT user2_br FOREIGN KEY(user2) REFERENCES users(u_id),
	winner		char(36),
	CONSTRAINT winner_br FOREIGN KEY(winner) REFERENCES users(u_id),
	battletime 	timestamp NOT NULL,
	price		char(36) NOT NULL
);

CREATE TABLE auth_token(
	u_id char(36) NOT NULL PRIMARY KEY,
	CONSTRAINT fk_user_token FOREIGN KEY(u_id) REFERENCES users(u_id),
	valid_until timestamp NOT NULL
);

CREATE TABLE deck_card(
	d_id char(36) NOT NULL,
	CONSTRAINT fk_stack FOREIGN KEY(d_id) REFERENCES decks(d_id),
	c_id char(36) NOT NULL,
	CONSTRAINT fk_card FOREIGN KEY(c_id) REFERENCES cards(c_id),
	creationtime timestamp NOT NULL,
	PRIMARY KEY(d_id, c_id)
);