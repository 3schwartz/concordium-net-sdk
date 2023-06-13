﻿namespace Concordium.Sdk.Types.New;

public abstract record Level1Update;

public record Level1KeysLevel1Update(
    HigherLevelAccessStructureLevel1Keys Content) : Level1Update;

public record Level2KeysLevel1Update(
    AuthorizationsV0 Content) : Level1Update;

public record Level2KeysV1Level1Update(
    AuthorizationsV1 Content) : Level1Update;