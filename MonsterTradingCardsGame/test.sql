CREATE TABLE IF NOT EXISTS users (
    u_id        char(36) PRIMARY KEY,
    username    varchar(40) NOT NULL UNIQUE,
    u_password  varchar(64) NOT NULL,
    coins   	integer NOT NULL,
	u_description varchar(128),
	elo			integer NOT NULL,
	CHECK(coins >= 0)
);

CREATE TABLE IF NOT EXISTS decks (
	d_id 			char(36) PRIMARY KEY,
	d_description 	char(128) NOT NULL,
	creationtime 	timestamp NOT NULL,
	u_id			char(36) NOT NULL,
	CONSTRAINT fk_deck_user FOREIGN KEY(u_id) REFERENCES users(u_id) 
);

CREATE TABLE IF NOT EXISTS user_selected_deck (
	u_id	char(36) PRIMARY KEY,
	d_id 	char(36) NOT NULL,
	CONSTRAINT fk_user_selected_deck_user FOREIGN KEY(u_id) REFERENCES users(u_id),
	CONSTRAINT fk_user_selected_deck_deck FOREIGN KEY(d_id) REFERENCES decks(d_id)  
);

CREATE TABLE IF NOT EXISTS packages (
    p_id        	char(36) PRIMARY KEY,
    p_description 	varchar(128) NOT NULL,
	creationtime 	timestamp NOT NULL,
	price			integer NOT NULL,
	buyer			char(36) NOT NULL,
	CONSTRAINT fk_packages_user FOREIGN KEY(buyer) REFERENCES users(u_id),
	CHECK(price >= 0)
);

CREATE TABLE IF NOT EXISTS cards (
    c_id        char(36) PRIMARY KEY,
    c_description varchar(128) NOT NULL,
	c_kind		e_kind NOT NULL,
	c_type		e_type NOT NULL,
	c_element	e_element NOT NULL,
	creationtime timestamp NOT NULL,
	damage integer NOT NULL,
	u_id	char(36) NOT NULL,
	p_id 	char(36) NOT NULL,
	CONSTRAINT fk_card_user FOREIGN KEY(u_id) REFERENCES users(u_id),
	CONSTRAINT fk_card_package FOREIGN KEY(p_id) REFERENCES packages(p_id),
	CHECK(damage >= 0)
);

CREATE TABLE IF NOT EXISTS battleResults (
    br_id       char(36) PRIMARY KEY,
    user1		char(36) NOT NULL,
	CONSTRAINT user1_br FOREIGN KEY(user1) REFERENCES users(u_id),
	user2		char(36) NOT NULL,
	CONSTRAINT user2_br FOREIGN KEY(user2) REFERENCES users(u_id),
	winner		char(36),
	CONSTRAINT winner_br FOREIGN KEY(winner) REFERENCES users(u_id),
	battletime 	timestamp NOT NULL
);

CREATE TABLE IF NOT EXISTS auth_token(
	u_id char(36) NOT NULL PRIMARY KEY,
	CONSTRAINT fk_user_token FOREIGN KEY(u_id) REFERENCES users(u_id),
	valid_until timestamp NOT NULL
);

CREATE TABLE IF NOT EXISTS deck_card(
	d_id char(36) NOT NULL,
	CONSTRAINT fk_stack FOREIGN KEY(d_id) REFERENCES decks(d_id),
	c_id char(36) NOT NULL,
	CONSTRAINT fk_deck_card FOREIGN KEY(c_id) REFERENCES cards(c_id),
	creationtime timestamp NOT NULL,
	PRIMARY KEY(d_id, c_id)
);

CREATE TABLE IF NOT EXISTS trading_offer(
	c_id char(36) PRIMARY KEY,
	seller char(36) NOT NULL,
	CONSTRAINT fk_trading_offer_seller FOREIGN KEY(seller) REFERENCES users(u_id),
	CONSTRAINT fk_trading_offer_card FOREIGN KEY(c_id) REFERENCES cards(c_id),
	creationtime timestamp NOT NULL,
	desiredType e_type NOT NULL,
	minDamage integer NOT NULL,
	CHECK(minDamage >= 0)
);

CREATE TABLE IF NOT EXISTS selling_offer(
	c_id char(36) PRIMARY KEY,
	seller char(36) NOT NULL,
	CONSTRAINT fk_selling_offer_seller FOREIGN KEY(seller) REFERENCES users(u_id),
	CONSTRAINT fk_selling_offer_card FOREIGN KEY(c_id) REFERENCES cards(c_id),
	creationtime timestamp NOT NULL,
	price 	integer NOT NULL,
	CHECK(price >= 0)
);

CREATE TABLE IF NOT EXISTS trading_record(
	tr_id	char(36) PRIMARY KEY,
	buyer	char(36) NOT NULL,
	seller	char(36) NOT NULL,
	buyer_card_id char(36) NOT NULL,
	seller_card_id char(36) NOT NULL,
	creationtime timestamp NOT NULL,
	CONSTRAINT fk_trading_record_buyer FOREIGN KEY(buyer) REFERENCES users(u_id),
	CONSTRAINT fk_trading_record_seller FOREIGN KEY(seller) REFERENCES users(u_id),
	CONSTRAINT fk_trading_record_buyer_card FOREIGN KEY(buyer_card_id) REFERENCES cards(c_id),
	CONSTRAINT fk_trading_record_seller_card FOREIGN KEY(seller_card_id) REFERENCES cards(c_id)
);

CREATE TABLE IF NOT EXISTS selling_record(
	sr_id	char(36) PRIMARY KEY,
	buyer	char(36) NOT NULL,
	seller	char(36) NOT NULL,
	seller_card_id char(36) NOT NULL,
	creationtime timestamp NOT NULL,
	price 	integer NOT NULL,
	CHECK(price >= 0),
	CONSTRAINT fk_selling_record_buyer FOREIGN KEY(buyer) REFERENCES users(u_id),
	CONSTRAINT fk_selling_record_seller FOREIGN KEY(seller) REFERENCES users(u_id),
	CONSTRAINT fk_selling_record_seller_card FOREIGN KEY(seller_card_id) REFERENCES cards(c_id)
);

