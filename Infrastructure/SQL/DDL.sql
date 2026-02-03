
DROP TABLE IF EXISTS Transferencias, Transacoes,Contas,Usuarios;
DROP TABLE IF EXISTS UsuariosLog, ContasLog;

CREATE TABLE Usuarios (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    Status INT NOT NULL,
    Nome VARCHAR(100) NOT NULL,
    Tipo INT NOT NULL, -- ADM, Gerente, Cliente
    CPF VARCHAR(14) UNIQUE NOT NULL,
    Email VARCHAR(100) UNIQUE NOT NULL,
    Hash VARCHAR(255),
    Salt VARBINARY(128),
    CriadoEm DATETIME2 DEFAULT GETDATE(),
    AtualizadoEm DATETIME2 DEFAULT GETDATE(),
    AtualizadoPor VARCHAR(255),
    DeletadoEm DATETIME2 NULL
);

CREATE TABLE Contas (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    IdUsuarioCliente BIGINT NOT NULL,
    IdUsuarioGerente BIGINT NOT NULL,
    Codigo VARCHAR(4) UNIQUE NOT NULL,
    Saldo DECIMAL(18, 2) DEFAULT 0,
    Reservado DECIMAL(18, 2) DEFAULT 0,
    LimiteCredito DECIMAL(18, 2) DEFAULT 0,
    SaldoCredito DECIMAL(18, 2) DEFAULT 0,
    Status INT NOT NULL,
    CriadoEm DATETIME2 DEFAULT GETDATE(),
    AtualizadoEm DATETIME2 DEFAULT GETDATE(),
    AtualizadoPor VARCHAR(255),
    DeletadoEm DATETIME2 NULL
    
    -- Relacionamentos
    CONSTRAINT FK_Conta_Cliente FOREIGN KEY (IdUsuarioCliente) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Conta_Gerente FOREIGN KEY (IdUsuarioGerente) REFERENCES Usuarios(Id)
);

CREATE TABLE Transacoes (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    IdUsuarioCliente BIGINT NOT NULL,
    CodigoConta VARCHAR(4) NOT NULL,
    Tipo INT NOT NULL, -- Débito, Depósito, Aplicação, etc.
    Valor DECIMAL(18, 2) NOT NULL,
    ValorFinal DECIMAL(18, 2) NULL,
    StatusTransacao INT NOT NULL,
    CriadoEm DATETIME2 DEFAULT GETDATE(),
    AtualizadoEm DATETIME2 DEFAULT GETDATE(),
    AtualizadoPor VARCHAR(255),
    
    -- Relacionamentos
    CONSTRAINT FK_Transacao_Cliente FOREIGN KEY (IdUsuarioCliente) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Transacao_Conta FOREIGN KEY (CodigoConta) REFERENCES Contas(Codigo)
);

CREATE TABLE Transferencias (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    IdTransacao BIGINT NOT NULL,
    IdUsuarioClienteDestino BIGINT NOT NULL,
    CodigoContaDestino VARCHAR(4) NOT NULL, -- Conta de Destino
    ValorFinal DECIMAL(18, 2) NULL,
    
    -- Relacionamentos
    CONSTRAINT FK_Transferencia_Transacao FOREIGN KEY (IdTransacao) REFERENCES Transacoes(Id),
    CONSTRAINT FK_Transferencia_Cliente FOREIGN KEY (IdUsuarioClienteDestino) REFERENCES Usuarios(Id),
    CONSTRAINT FK_Transferencia_ContaDestino FOREIGN KEY (CodigoContaDestino) REFERENCES Contas(Codigo)
);

CREATE TABLE UsuariosLog (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    TipoOperacao CHAR(6),
    IdUsuario BIGINT,
    Status INT,
    Nome VARCHAR(100),
    Tipo INT,
    CPF VARCHAR(14),
    Email VARCHAR(100),
    Hash VARCHAR(255),
    Salt VARBINARY(128),
    CriadoEm DATETIME2,
    AtualizadoEm DATETIME2,
    AtualizadoPor VARCHAR(255),
    DeletadoEm DATETIME2 NULL
);

CREATE TABLE ContasLog (
    Id BIGINT PRIMARY KEY IDENTITY(1,1),
    TipoOperacao CHAR(6),
    IdConta BIGINT ,
    IdUsuarioCliente BIGINT,
    IdUsuarioGerente BIGINT,
    Codigo VARCHAR(4),
    Saldo DECIMAL(18, 2),
    Reservado DECIMAL(18, 2),
    LimiteCredito DECIMAL(18, 2),
    SaldoCredito DECIMAL(18, 2),
    Status INT,
    CriadoEm DATETIME2 ,
    AtualizadoEm DATETIME2 ,
    AtualizadoPor VARCHAR(255),
    DeletadoEm DATETIME2 NULL
);
GO

CREATE OR ALTER TRIGGER TR_UsuarioLog 
ON Usuarios
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tipoOperacao CHAR(6);

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        IF((SELECT DeletadoEm FROM inserted) IS NOT NULL)
        BEGIN
            SET @tipoOperacao = 'DELETE';
        END
        ELSE
        BEGIN
            SET @tipoOperacao = 'UPDATE';
        END
    END
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Only inserted has data, so it's an INSERT
        SET @tipoOperacao = 'INSERT';
    END
    ELSE IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Only deleted has data, so it's a DELETE
        SET @tipoOperacao = 'DELETE';
    END;

    INSERT INTO UsuariosLog(
        TipoOperacao,
        IdUsuario, 
        Status, 
        Nome, 
        Tipo, 
        CPF, 
        Email, 
        Hash, 
        Salt, 
        CriadoEm, 
        AtualizadoEm, 
        AtualizadoPor, 
        DeletadoEm) 
    SELECT
        @tipoOperacao,
        Inserted.ID,
        Inserted.Status, 
        Inserted.Nome, 
        Inserted.Tipo, 
        Inserted.CPF, 
        Inserted.Email, 
        Inserted.Hash, 
        Inserted.Salt, 
        Inserted.CriadoEm, 
        Inserted.AtualizadoEm, 
        Inserted.AtualizadoPor, 
        Inserted.DeletadoEm
    FROM
        Inserted;
END;

GO
---------------------------------------------------------------------------------------------------------------------------------------------------------------------
INSERT INTO Usuarios (Status, Nome, Tipo, CPF, Email, Hash, Salt, AtualizadoPor) VALUES 
(1, 'Administrador', 1, '111', 'administrador@pagueveloz.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(1, 'Renata', 1, '222', 'renata@pagueveloz.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(2, 'Rogério', 1, '333', 'rogerio@pagueveloz.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(1, 'Márcia', 2, '444', 'marcia@pagueveloz.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(1, 'Lucas', 2, '555', 'lucas@pagueveloz.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(1, 'José', 3, '666', 'jose@gmail.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(2, 'Rose', 3, '777', 'rose@hotmail.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(1, 'Paulo', 3, '888', 'paulo@yahoo.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA'),
(1, 'Mônica', 3, '999', 'monica@bol.com', '52Rg3D6rqoi3JimTpKYuKesKAL4w9XmLU95MeL5IhcY=', 0x1C85E256B64B75FB4419B7A145769F67, 'SISTEMA');

GO

CREATE OR ALTER TRIGGER TR_ContaLog 
ON Contas
AFTER INSERT, UPDATE, DELETE
AS
BEGIN
    SET NOCOUNT ON;

    DECLARE @tipoOperacao CHAR(6);

    IF EXISTS (SELECT * FROM inserted) AND EXISTS (SELECT * FROM deleted)
    BEGIN
        IF((SELECT DeletadoEm FROM inserted) IS NOT NULL)
        BEGIN
            SET @tipoOperacao = 'DELETE';
        END
        ELSE
        BEGIN
            SET @tipoOperacao = 'UPDATE';
        END
    END
    ELSE IF EXISTS (SELECT * FROM inserted)
    BEGIN
        -- Only inserted has data, so it's an INSERT
        SET @tipoOperacao = 'INSERT';
    END
    ELSE IF EXISTS (SELECT * FROM deleted)
    BEGIN
        -- Only deleted has data, so it's a DELETE
        SET @tipoOperacao = 'DELETE';
    END;

    INSERT INTO ContasLog(
        TipoOperacao,
        IdConta,
        IdUsuarioCliente,
        IdUsuarioGerente,
        Codigo,
        Saldo,
        Reservado,
        LimiteCredito,
        SaldoCredito,
        Status,
        CriadoEm,
        AtualizadoEm,
        AtualizadoPor,
        DeletadoEm)
    SELECT
        @tipoOperacao,
        Inserted.ID,
        Inserted.IdUsuarioCliente,
        Inserted.IdUsuarioGerente,
        Inserted.Codigo,
        Inserted.Saldo,
        Inserted.Reservado,
        Inserted.LimiteCredito,
        Inserted.SaldoCredito,
        Inserted.Status,
        Inserted.CriadoEm,
        Inserted.AtualizadoEm,
        Inserted.AtualizadoPor,
        Inserted.DeletadoEm
    FROM
        Inserted;
END;

